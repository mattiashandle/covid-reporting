import * as React from "react";
import { Container, Row, Form, Button, Col, Alert } from "react-bootstrap";
import {
  HealthcareProviderDto,
  CreateCapacityReportCommand,
  ICreateCapacityReportCommand
} from "../SDKs/api.generated.clients";
import { useState } from "react";
import CapacityReportTable from "../tables/CapacityReportTable";
import ClientFactory from "../SDKs/ClientFactory";

type Props = {
  provider: HealthcareProviderDto;
};

function CapacityReportForm(props: Props) {
  const [capacityDate, setCapacityDate] = useState<Date>();
  const [numberOfDoses, setNumberOfDoses] = useState(0);
  const [submit, setSubmit] = useState(false);  
  const [buttonDisabled, setButtonDisabled] = React.useState(false);

  const client = new ClientFactory().CreateProviderClient();

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {

    setButtonDisabled(true);

    const command : ICreateCapacityReportCommand = {
      date: capacityDate,
      numberOfDoses: numberOfDoses,
      healthcareProviderId: props.provider.id!
    }

    e.preventDefault();

    client.addCapacityReport(props.provider.id!, new CreateCapacityReportCommand(command)).then((response) => {
        if(response){
          setSubmit(true);
          setButtonDisabled(false);
        }
    }, (error) => {console.log(error)});

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
              <Col md={6}>
                <Form.Group className="mb-1" controlId="capacityDateInput">
                  <Form.Label>Kapacitets datum</Form.Label>
                  <Form.Control
                    onChange={(e) =>
                      setCapacityDate(new Date(e.target.value + "T00:00"))
                    }
                    type="date"
                  />
                </Form.Group>
              </Col>
              <Col md={2}>
                <Form.Group className="mb-3" controlId="numberOfDosesInput">
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
          <CapacityReportTable provider={props.provider!} />
        </Row>
      </Container>
    </>
  );
}

export default CapacityReportForm;
