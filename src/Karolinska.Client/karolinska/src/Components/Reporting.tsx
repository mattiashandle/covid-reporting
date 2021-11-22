import * as React from "react";
import { Container, Row, Button, Dropdown, Form, Card } from "react-bootstrap";
import {
  CapacityReportDto,
  ExpenditureReportDto,
  HealthcareProviderClient,
  HealthcareProviderDto,
  OrderReportDto,
  ReceiptReportDto,
  StockBalanceReportDto,
} from "./SDKs/api.generated.clients";
import "../App.css";
import { Link } from "react-router-dom";

type State = {
  isLoaded: boolean;
  client: HealthcareProviderClient
  providers: HealthcareProviderDto[]
};

export default class Reporting extends React.Component {
  options: HealthcareProviderDto[] = [];
  state: State = {
    isLoaded: false,
    client: new HealthcareProviderClient("http://localhost:5271"),
    providers: []
  };
  componentDidMount() {
    const client = this.state.client;

    client.getHealthcareProviders(1, 100).then((response) => {
      this.setState({providers: response.data!, isLoaded: true})
    })


  }
  render() {
    return (
      <>
       <Container>
         <Row>
           <h2 className="text-center">Välj en vårdgivare nedan</h2>
         </Row>

        {this.state.providers.map((provider) => {
          return <Row className="mt-5">
                  <Card className="text-center">
                    <Card.Header>Vårdgivare</Card.Header>
                    <Card.Body>
                      <Card.Title>{provider.name}</Card.Title>
                      <Card.Text>
                      Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo.
                      </Card.Text>
                   
                      <Button href={'/reports/add?id=' + provider.id} variant="primary">Rapportera in</Button>
                    </Card.Body>
                  </Card>
                  </Row>  
        })}

        
     </Container>
      </>
    )
  }
}
  