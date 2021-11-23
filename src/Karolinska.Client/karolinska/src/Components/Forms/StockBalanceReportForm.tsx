import * as React from "react";
import { Container, Row, Form, Button, Col, Alert } from "react-bootstrap";
import {
  HealthcareProviderDto,
  CreateStockBalanceReportCommand,
  ICreateStockBalanceReportCommand,
  SupplierDto
} from "../SDKs/api.generated.clients";
import { useState, useEffect } from "react";
import StockBalanceReportTable from "../tables/StockBalanceReportTable";
import Select from "react-select";
import ClientFactory from "../SDKs/ClientFactory";

type Props = {
  provider: HealthcareProviderDto;
};

function StockBalanceReportForm(props: Props) {
  const [stockDate, setStockDate] = useState<Date>();
  const [numberOfVials, setNumberOfVials] = useState(0);
  const [numberOfDoses, setNumberOfDoses] = useState(0);
  const [submit, setSubmit] = useState(false);  
  const [buttonDisabled, setButtonDisabled] = useState(false);
  const [loading, setLoading] = useState(true);
  const [suppliers, setSuppliers] = useState<SupplierDto[]>();
  const [selectedSupplier, setSelectedSupplier] = useState<SupplierDto | null>(null);

  const supplierClient = new ClientFactory().CreateSupplierClient();

  useEffect(() => {
      if(!loading){return;}

      supplierClient.getSuppliers(1, 100).then((response) => {
        setSuppliers(response.data);
        setSelectedSupplier(response.data![0]!);
        setLoading(false);
      });    
  })

  const handleSupplierChange = (
    selected: any
  ) => {
    console.log(selected)
    if (selected instanceof SupplierDto) {
        setSelectedSupplier(selected)
    }
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {

    setButtonDisabled(true);

    if(!selectedSupplier) return;
    
    if(!stockDate) return;

    const command : ICreateStockBalanceReportCommand = {
      date: stockDate,
      supplierId: selectedSupplier.id,
      numberOfVials: numberOfVials,
      numberOfDoses: numberOfDoses,
      healthcareProviderId: props.provider.id
    }

    console.log(command)

    e.preventDefault();

    const providerClient = new ClientFactory().CreateProviderClient();

    providerClient.addStockBalanceReport(props.provider.id!, new CreateStockBalanceReportCommand(command)).then((response) => {
        console.log(response);
        setSubmit(true);
        setButtonDisabled(false);
    }, (error) => console.log(error)).catch((exception) => console.log(exception));

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
                <Form.Group className="mb-1" controlId="stockDateInput">
                  <Form.Label>Datum tid</Form.Label>
                  <Form.Control
                    onChange={(e) =>
                      setStockDate(new Date(e.target.value + "T00:00"))
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
                <Form.Group className="mb-3" controlId="numberOfVials">
                  <Form.Label>Kvantitet (vial)</Form.Label>
                  <Form.Control
                    required={true}
                    onChange={(e) => setNumberOfVials(parseInt(e.target.value))}
                    type="number"
                  />
                </Form.Group>
              </Col>

              <Col md={3}>
                <Form.Group className="mb-3" controlId="numberOfDoses">
                  <Form.Label>Kvantitet (doser)</Form.Label>
                  <Form.Control
                    required={true}
                    onChange={(e) => setNumberOfDoses(parseInt(e.target.value))}
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
          <StockBalanceReportTable provider={props.provider!} />
        </Row>
      </Container>
    </>
  );
}

export default StockBalanceReportForm;
