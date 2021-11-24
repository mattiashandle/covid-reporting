import * as React from "react";
import { Container, Row, Form, Button, Col, Alert } from "react-bootstrap";
import {
  HealthcareProviderDto,
  CreateOrderReportCommand,
  ICreateOrderReportCommand,
  OrderReportDto
} from "../sdk/api.generated.clients";
import { useState, useEffect } from "react";
import OrderReportTableV2 from "../tables/OrderReportTableV2";
import ClientFactory from "../sdk/ClientFactory";

type Props = {
  provider: HealthcareProviderDto;
};

type FormState = {
  reports?: OrderReportDto[],
  orderDate?: Date,
  requestedDeliveryDate?: Date,
  quantity?: number,
  glnReceiver? : string
}

function OrderReportForm(props: Props) {
  const [submit, setSubmit] = useState(false);  
  const [buttonDisabled, setButtonDisabled] = useState(false);
  const [formState, setFormState] = useState<FormState>({reports:[], quantity: 0, glnReceiver: '', orderDate: new Date(), requestedDeliveryDate: new Date()});
  const providerClient = new ClientFactory().CreateProviderClient();

  useEffect(() => {
    async function fetch() {
      let expenditureReports = await providerClient.getOrderReports(props.provider?.id!, 1, 100);
      setFormState(prevState => { return {
        ...prevState,
        reports: expenditureReports.data
        }
      })
    }
    fetch();
  }, [submit])


  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setButtonDisabled(true);

    const command : ICreateOrderReportCommand = {
      orderDate: formState.orderDate,
      requestedDeliveryDate: formState.requestedDeliveryDate,
      quantity: formState.quantity,
      glnReceiver: formState.glnReceiver,
      healthcareProviderId: props.provider.id!
    }
    console.log(command);
    providerClient.addOrderReport(props.provider.id!, new CreateOrderReportCommand(command)).then(() => {
        setSubmit(true);
        setButtonDisabled(false);
      }, (error) => {console.log(error)});
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
                <Form.Group className="mb-1" controlId="orderDateInput">
                  <Form.Label>Beställnings datum</Form.Label>
                  <Form.Control
                    onChange={(e) =>
                      setFormState(prevState => { return {...prevState, orderDate: new Date(e.target.value)} })
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
                  <Form.Label>Önskat lev datum</Form.Label>
                  <Form.Control
                    required={true}
                    onChange={(e) =>
                      setFormState(prevState => { return {...prevState, requestedDeliveryDate: new Date(e.target.value)} })
                    }
                    type="date"
                  />
                </Form.Group>
              </Col>
              <Col md={3}>
                <Form.Group className="mb-3" controlId="quantityInput">
                  <Form.Label>Kvantitet (dos)</Form.Label>
                  <Form.Control
                    required={true}
                    onChange={(e) => setFormState(prevState => { return {...prevState, quantity: parseInt(e.target.value)} })}
                    type="number"
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
          <OrderReportTableV2 reports={formState.reports} />
        </Row>
      </Container>
    </>
  );
}

export default OrderReportForm;
