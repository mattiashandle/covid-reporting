import * as React from "react";
import { Container, Row, Form, Button, Col, Alert } from "react-bootstrap";
import {
  HealthcareProviderDto,
  CreateReceiptReportCommand,
  ICreateReceiptReportCommand,
  SupplierDto,
  ReceiptReportDto
} from "../sdk/api.generated.clients";
import { useState, useEffect } from "react";
import ReceiptReportTable from "../tables/ReceiptReportTable";
import Select from "react-select";
import ClientFactory from "../sdk/ClientFactory";

type Props = {
  provider: HealthcareProviderDto;
};

type FormState = {
  reports?: ReceiptReportDto[],
  deliveryDate?: Date,
  expectedDeliveryDate?: Date,
  numberOfVials?: number,
  glnReceiver? : string,
  suppliers?: SupplierDto[],
  selectedSupplier? : SupplierDto,
}


function ReceiptReportForm(props: Props) {
  const [submit, setSubmit] = useState(false);  
  const [buttonDisabled, setButtonDisabled] = React.useState(false);
  const [formState, setFormState] = useState<FormState>({
    reports: [], 
    deliveryDate: new Date(), 
    expectedDeliveryDate: new Date(), 
    numberOfVials: 0, 
    glnReceiver: '',
    suppliers: [],
    selectedSupplier: undefined })

  const clientFactory = new ClientFactory();
  const supplierClient = clientFactory.CreateSupplierClient();
  const providerClient = clientFactory.CreateProviderClient();

  useEffect(() => {
    async function fetch() {
      let supplierResponse = await supplierClient.getSuppliers(1, 100);
      let receiptReports = await providerClient.getReceiptReports(props.provider?.id!, 1, 100);
      setFormState(prevState => { return {
        ...prevState,
        reports: receiptReports.data,
        suppliers: supplierResponse.data, 
        selectedSupplier: supplierResponse.data![0]
        }
      })
    }
    fetch();
  }, [submit])

  const handleSupplierChange = (
    selected?: SupplierDto | SupplierDto[] | null
  ) => {
    if (selected instanceof SupplierDto) {
      setFormState(prevState => { return {
        ...prevState,
        selectedSupplier: selected
        }
      })
    }
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setButtonDisabled(true);

    const command : ICreateReceiptReportCommand = {
      deliveryDate: formState.deliveryDate,
      expectedDeliveryDate: formState.expectedDeliveryDate,
      numberOfVials: formState.numberOfVials,
      glnReceiver: formState.glnReceiver,
      healthcareProviderId: props.provider.id!,
      supplierId: formState?.selectedSupplier!.id!
    }

    const providerClient = new ClientFactory().CreateProviderClient();

    providerClient.addReceiptReport(props.provider.id!, new CreateReceiptReportCommand(command)).then(() => {
        setSubmit(true);
        setButtonDisabled(false);
    }, (error) => {console.log(error)}).catch((error) => console.log(error));
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
                      setFormState(prevState => { return {...prevState, deliveryDate: new Date(e.target.value)} })
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
                      setFormState(prevState => { return {...prevState, expectedDeliveryDate: new Date(e.target.value)} })
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
                    onChange={(e) => setFormState(prevState => { return {...prevState, numberOfVials: parseInt(e.target.value)} })}
                    type="number"
                  />
                </Form.Group>
              </Col>

              <Col md={3}>
                <Form.Group className="mb-3" controlId="supplierInput">
                  <Form.Label>Vaccinleverantör</Form.Label>
                     <Select
                        value={formState.selectedSupplier}
                        onChange={handleSupplierChange}
                        options={formState.suppliers}
                        getOptionLabel={(supplier: SupplierDto) => supplier.name!}
                        getOptionValue={(supplier: SupplierDto) => supplier.id!}
                        />
                </Form.Group>
              </Col>

              <Col md={3}>
                <Form.Group className="mb-3" controlId="glnReceiverInput">
                  <Form.Label>GLN Mottagare</Form.Label>
                  <Form.Control
                    onChange={(e) => setFormState(prevState => { return {...prevState, glnReceiver: e.target.value} })}
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
