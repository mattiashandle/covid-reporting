import * as React from "react";
import { Container, Row, Form, Button, Col, Alert } from "react-bootstrap";
import {
  HealthcareProviderDto,
  CreateStockBalanceReportCommand,
  ICreateStockBalanceReportCommand,
  SupplierDto,
  StockBalanceReportDto
} from "../sdk/api.generated.clients";
import { useState, useEffect } from "react";
import StockBalanceReportTable from "../tables/StockBalanceReportTable";
import Select from "react-select";
import ClientFactory from "../sdk/ClientFactory";

type Props = {
  provider: HealthcareProviderDto;
};

type FormState = {
  reports?: StockBalanceReportDto[],
  suppliers?: SupplierDto[],
  selectedSupplier? : SupplierDto,
  stockDate?: Date,
  numberOfVials?: number,
  numberOfDoses?: number
}

function StockBalanceReportForm(props: Props) {
  const [submit, setSubmit] = useState(false);  
  const [buttonDisabled, setButtonDisabled] = useState(false);
  const [formState, setFormState] = useState<FormState>({reports: [], suppliers: [], stockDate: new Date(), numberOfVials: 0, numberOfDoses: 0})
  const clientFactory = new ClientFactory();
  const providerClient = clientFactory.CreateProviderClient();
  const supplierClient = clientFactory.CreateSupplierClient();

  useEffect(() => {
    async function fetch() {
      let supplierResponse = await supplierClient.getSuppliers(1, 100);
      let stockBalanceReports = await providerClient.getStockBalanceReports(props.provider?.id!, 1, 100);
      setFormState(prevState => { return {
        ...prevState,
        reports: stockBalanceReports.data,
        suppliers: supplierResponse.data, 
        selectedSupplier: supplierResponse.data![0]
        }
      })
    }
    fetch();
  }, [submit])


  const handleSupplierChange = (
    selected: any
  ) => {
    console.log(selected)
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
    e.stopPropagation();
    setButtonDisabled(true);

    const command : ICreateStockBalanceReportCommand = {
      date: formState.stockDate,
      supplierId: formState.selectedSupplier?.id!,
      numberOfVials: formState.numberOfVials,
      numberOfDoses: formState.numberOfDoses,
      healthcareProviderId: props.provider.id
    }

    console.log(command)

    providerClient.addStockBalanceReport(props.provider.id!, new CreateStockBalanceReportCommand(command)).then((response) => {
        console.log(response);
        setSubmit(true);
        setButtonDisabled(false);
    }, (error) => console.log(error)).catch((exception) => console.log(exception));
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
                      setFormState(prevState => { return {...prevState, stockDate: new Date(e.target.value)} })
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
                <Form.Group className="mb-3" controlId="numberOfDoses">
                  <Form.Label>Kvantitet (doser)</Form.Label>
                  <Form.Control
                    required={true}
                    onChange={(e) => setFormState(prevState => { return {...prevState, numberOfDoses: parseInt(e.target.value)} })}
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
