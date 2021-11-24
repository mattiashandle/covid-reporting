import * as React from "react";
import {useEffect} from "react";
// import ReactDOM from "react-dom";
import { Container, Row, Form, Button, Col, Alert } from "react-bootstrap";
import {
  HealthcareProviderDto,
  CreateCapacityReportCommand,
  ICreateCapacityReportCommand,
  CapacityReportDto
} from "../sdk/api.generated.clients";
import { useState, useRef } from "react";
import ClientFactory from "../sdk/ClientFactory";
import CapacityReportTableV2 from "../tables/CapacityReportTableV2";

type Props = {
  provider: HealthcareProviderDto;
};

function CapacityReportForm(props: Props) {
  const [capacityDate, setCapacityDate] = useState<Date | undefined>();
  const [numberOfDoses, setNumberOfDoses] = useState(0);
  const [submit, setSubmit] = useState(false);  
  const [buttonDisabled, setButtonDisabled] = React.useState(false);
  const formRef = useRef<HTMLFormElement | null>(null);
  const [capacityReports, setCapacityReports] = useState<CapacityReportDto[] | null>(null);
  const [hasFetched, setHasFetched] = useState(false);
  
  const client = new ClientFactory().CreateProviderClient();

  useEffect(() => {
    console.log("useEffect triggered from CapacityReportForm.tsx")
    console.log(hasFetched)
    if(!hasFetched){
      console.log("fetching capacityReports")
      client.getCapacityReports(props.provider.id!, 1, 100).then((response) => {
        setHasFetched(true);
        setCapacityReports(response.data!)
      })
    }
  })

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    setButtonDisabled(true);
    e.preventDefault();
    e.stopPropagation();

    const command : ICreateCapacityReportCommand = {
      date: capacityDate,
      numberOfDoses: numberOfDoses,
      healthcareProviderId: props.provider.id!
    }

    client.addCapacityReport(props.provider.id!, new CreateCapacityReportCommand(command)).then((response) => {
        if(response){
          setSubmit(true);
          setButtonDisabled(false);
          setCapacityReports([response, ...capacityReports!])
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
          <Form ref={formRef}
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
          <CapacityReportTableV2 reports={capacityReports} />
        </Row>
      </Container>
    </>
  );
}

export default CapacityReportForm;
