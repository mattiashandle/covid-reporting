import * as React from "react";
import { Container, Row, Form, Button, Col, Alert } from "react-bootstrap";
import {
  HealthcareProviderDto,
  CreateReceiptReportCommand,
  ICreateReceiptReportCommand,
  SupplierDto
} from "../SDKs/api.generated.clients";
import { useState, useEffect } from "react";
import ReceiptReportTable from "../tables/ReceiptReportTable";
import Select from "react-select";
import ClientFactory from "../SDKs/ClientFactory";

type Props = {
  provider: HealthcareProviderDto;
};

function ReceiptReportForm(props: Props) {
  const [deliveryDate, setDeliveryDate] = useState<Date>();
  const [expectedDeliveryDate, setExpectedDeliveryDate] = useState<Date>();
  const [numberOfVials, setNumberOfVials] = useState(0);
  const [glnReceiver, setGlnReceiver] = useState("");
  const [submit, setSubmit] = useState(false);  
  const [buttonDisabled, setButtonDisabled] = React.useState(false);
  const [loading, setLoading] = useState(true);
  const [suppliers, setSuppliers] = useState<SupplierDto[]>();
  const [selectedSupplier, setSelectedSupplier] = useState<SupplierDto>();


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
    selected?: SupplierDto | SupplierDto[] | null
  ) => {
    if (selected instanceof SupplierDto) {
        setSelectedSupplier(selected)
    }
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {

    setButtonDisabled(true);

    if(!selectedSupplier)
      return;

    const command : ICreateReceiptReportCommand = {
      deliveryDate: deliveryDate,
      expectedDeliveryDate: expectedDeliveryDate,
      numberOfVials: numberOfVials,
      glnReceiver: glnReceiver,
      healthcareProviderId: props.provider.id!,
      supplierId: selectedSupplier.id
    }

    e.preventDefault();

    const providerClient = new ClientFactory().CreateProviderClient();

    providerClient.addReceiptReport(props.provider.id!, new CreateReceiptReportCommand(command)).then(() => {
        setSubmit(true);
        setButtonDisabled(false);
    }, (error) => {console.log(error)}).catch((error) => console.log(error));

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
                <Form.Group className="mb-1" controlId="deliveryDateInput">
                  <Form.Label>Lev datum</Form.Label>
                  <Form.Control
                    onChange={(e) =>
                      setDeliveryDate(new Date(e.target.value + "T00:00"))
                    }
                    type="date"
                  />
                </Form.Group>
              </Col>
              <Col md={3}>
                <Form.Group
                  className="mb-3"
                  controlId="expectedDeliveryDateInput"
                >
                  <Form.Label>Planerat lev datum</Form.Label>
                  <Form.Control
                    required={true}
                    onChange={(e) =>
                      setExpectedDeliveryDate(new Date(e.target.value + "T00:00"))
                    }
                    type="date"
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
                <Form.Group className="mb-3" controlId="glnReceiverInput">
                  <Form.Label>GLN Mottagare</Form.Label>
                  <Form.Control
                    onChange={(e) => setGlnReceiver(e.target.value)}
                    type="text"
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
          <ReceiptReportTable provider={props.provider!} />
        </Row>
      </Container>
    </>
  );
}

export default ReceiptReportForm;
