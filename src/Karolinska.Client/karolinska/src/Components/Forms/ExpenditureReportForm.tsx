import * as React from "react";
import { Container, Row, Form, Button, Col, Alert } from "react-bootstrap";
import {
  HealthcareProviderDto,
  SupplierDto,
  CreateExpenditureReportCommand,
  ICreateExpenditureReportCommand,
  ExpenditureReportDto
} from "../sdk/api.generated.clients";
import { useState, useEffect } from "react";
import Select from "react-select";
import ClientFactory from "../sdk/ClientFactory";
import ExpenditureReportTableV2 from "../tables/ExpenditureReportTableV2";

type Props = {
  provider: HealthcareProviderDto;
};

type FormState = {
  reports?: ExpenditureReportDto[],
  suppliers?: SupplierDto[],
  selectedSupplier? : SupplierDto,
  expenditureDate?: Date,
  numberOfVials?: number
}

function ExpenditureReportForm(props: Props) {
  const [submit, setSubmit] = useState(false);
  const [buttonDisabled, setButtonDisabled] = useState(false);
  const [formState, setFormState] = useState<FormState>({reports:[], suppliers: []});

  const clientFactory = new ClientFactory();
  const providerClient = clientFactory.CreateProviderClient();
  const supplierClient = clientFactory.CreateSupplierClient();

  useEffect(() => {
    async function fetch() {
      let supplierResponse = await supplierClient.getSuppliers(1, 100);
      let expenditureReports = await providerClient.getExpenditureReports(props.provider?.id!, 1, 100);
      setFormState(prevState => { return {
        ...prevState,
        reports: expenditureReports.data,
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
      console.log(formState)
      setFormState(prevState => { return {
        ...prevState,
        selectedSupplier: selected
        }
      })
      console.log(formState)
    }
  };
  
  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setButtonDisabled(true);

    const command : ICreateExpenditureReportCommand = {
      date: formState.expenditureDate,
      numberOfVials: formState.numberOfVials,
      supplierId: formState.selectedSupplier?.id!,
      healthcareProviderId: props.provider.id!
    }

    providerClient.addExpenditureReport(props.provider.id!, new CreateExpenditureReportCommand(command)).then(() => {
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
                <Form.Group className="mb-1" controlId="expenditureDateInput">
                  <Form.Label>Förbrukningsdatum</Form.Label>
                  <Form.Control
                    onChange={(e) =>
                      setFormState(prevState => { return {...prevState, expenditureDate: new Date(e.target.value)} })
                    }
                    type="date"
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
                <Form.Group
                  className="mb-3"
                  controlId="numberOfVialsInput"
                >
                  <Form.Label>Kvantitet (vial)</Form.Label>
                  <Form.Control
                    required={true}
                    onChange={(e) =>
                      setFormState(prevState => { return {...prevState, numberOfVials: parseInt(e.target.value)} })
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
          <ExpenditureReportTableV2 reports={formState.reports} />
        </Row>
      </Container>
    </>
  );
}

export default ExpenditureReportForm;
