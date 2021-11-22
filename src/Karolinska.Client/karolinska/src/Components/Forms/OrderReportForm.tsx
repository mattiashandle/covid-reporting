import * as React from "react";
import { Container, Row, Form, Button, Col, Alert } from "react-bootstrap";
import {
  HealthcareProviderDto,
  CreateOrderReportCommand,
} from "../SDKs/api.generated.clients";
import { useState } from "react";
import OrderReportTable from "../tables/OrderReportTable";
import ClientFactory from "../SDKs/ClientFactory";

type Props = {
  provider: HealthcareProviderDto;
};

function OrderReportForm(props: Props) {
  const [orderDate, setOrderDate] = useState<Date | null>(null);
  const [expectedDate, setExpectedDate] = useState<Date | null>(null);
  const [quantity, setQuantity] = useState(0);
  const [glnReceiver, setGlnReceiver] = useState("");
  const [submit, setSubmit] = useState(false);  
  const [buttonDisabled, setButtonDisabled] = React.useState(false);

  const client = new ClientFactory().CreateProviderClient();

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {

    setButtonDisabled(true);

    const command = CreateOrderReportCommand.fromJS({
      orderDate: orderDate,
      requestedDeliveryDate: expectedDate,
      quantity: quantity,
      glnReceiver: glnReceiver,
      healthcareProviderId: props.provider.id!,
    });

    e.preventDefault();

    client.addOrderReport(props.provider.id!, command).then((response) => {
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
                <Form.Group className="mb-1" controlId="orderDateInput">
                  <Form.Label>Beställnings datum</Form.Label>
                  <Form.Control
                    onChange={(e) =>
                      setOrderDate(new Date(e.target.value + "T00:00"))
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
                      setExpectedDate(new Date(e.target.value + "T00:00"))
                    }
                    type="date"
                  />
                </Form.Group>
              </Col>
              <Col md={3}>
                <Form.Group className="mb-3" controlId="numberOfDosesInput">
                  <Form.Label>Kvantitet (dos)</Form.Label>
                  <Form.Control
                    required={true}
                    onChange={(e) => setQuantity(parseInt(e.target.value))}
                    type="number"
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
          <OrderReportTable provider={props.provider!} />
        </Row>
      </Container>
    </>
  );
}

export default OrderReportForm;
