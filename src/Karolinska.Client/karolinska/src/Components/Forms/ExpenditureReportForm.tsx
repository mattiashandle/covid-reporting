import * as React from "react";
import { Container, Row, Form, Button, Col, Alert } from "react-bootstrap";
import {
  HealthcareProviderClient,
  HealthcareProviderDto,
  SupplierDto,
  CreateExpenditureReportCommand,
  SupplierClient,
} from "../SDKs/api.generated.clients";
import { useState, useEffect } from "react";
import EpenditureReportTable from "../tables/ExpenditureReportTable";
import Select from "react-select";
import ClientFactory from "../SDKs/ClientFactory";

type Props = {
  provider: HealthcareProviderDto;
};

function ExpenditureReportForm(props: Props) {
  const [expenditureDate, setExpenditureDate] = useState<Date | null>(null);
  const [numberOfVials, setNumberOfVials] = useState(0);
  const [submit, setSubmit] = useState(false);
  const [selectedSupplier, setSelectedSupplier] = useState<SupplierDto | null>(null);  
  const [buttonDisabled, setButtonDisabled] = useState(false);
  const [loading, setLoading] = useState(true);
  const [suppliers, setSuppliers] = useState<SupplierDto[]>();

  const client = new ClientFactory().CreateProviderClient();

  const supplierClient = new ClientFactory().CreateSupplierClient();

  useEffect(() => {
      if(!loading){return;}

      supplierClient.getSuppliers(1, 100).then((response) => {
        setSuppliers(response.data);
        setSelectedSupplier(response.data![0]);
        setLoading(false);
      });
    
  })

  const handleSupplierChange = (
    selected?: SupplierDto | SupplierDto[] | null
  ) => {
    if (selected instanceof SupplierDto) {
        setSelectedSupplier(selected)
    }
  };
  

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {

    setButtonDisabled(true);
    const command = CreateExpenditureReportCommand.fromJS({
      date: expenditureDate,
      numberOfVials: numberOfVials,
      supplierId: selectedSupplier?.id,
      healthcareProviderId: props.provider.id!,
    });

    e.preventDefault();

    client.addExpenditureReport(props.provider.id!, command).then((response) => {
        setSubmit(true);
        setButtonDisabled(false);
    });

    setTimeout(() => {
      setSubmit(false);
    },10000);
  };

  return (
    <>
      <Container>
      <Alert variant="success" show={submit}>Rapport skickad</Alert>
        <Row>
          <Form
            onSubmit={(e) => {
              handleSubmit(e);
            }}
          >
            <Row>
              <Col md={3}>
                <Form.Group className="mb-1" controlId="expenditureDateInput">
                  <Form.Label>Förbrukningsdatum</Form.Label>
                  <Form.Control
                    onChange={(e) =>
                      setExpenditureDate(new Date(e.target.value + "T00:00"))
                    }
                    type="date"
                  />
                </Form.Group>
              </Col>
              <Col md={3}>
                <Form.Group className="mb-3" controlId="supplierInput">
                  <Form.Label>Vaccinleverantör</Form.Label>
                     <Select
                        value={selectedSupplier}
                        onChange={handleSupplierChange}
                        options={suppliers}
                        getOptionLabel={(supplier: SupplierDto) => supplier.name!}
                        getOptionValue={(supplier: SupplierDto) => supplier.id!}
                        />
                </Form.Group>
              </Col>
              <Col md={3}>
                <Form.Group
                  className="mb-3"
                  controlId="numberOfVialsInput"
                >
                  <Form.Label>Kvantitet (vial)</Form.Label>
                  <Form.Control
                    required={true}
                    onChange={(e) =>
                      setNumberOfVials(parseInt(e.target.value))
                    }
                    type="number"
                  />
                </Form.Group>
              </Col>
              <Button disabled={buttonDisabled} className="mt-2" variant="primary" type="submit">
                Spara
              </Button>
            </Row>
          
          </Form>
        </Row>
       
        <Row className="mt-5">
          <EpenditureReportTable provider={props.provider!} />
        </Row>
      </Container>
    </>
  );
}

export default ExpenditureReportForm;
