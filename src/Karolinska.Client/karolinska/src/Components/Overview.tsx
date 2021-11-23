import * as React from "react";
import "../App.css";
import { Container, Row } from "react-bootstrap";
import { HealthcareProviderClient, HealthcareProviderDto } from "./SDKs/api.generated.clients";
import Select from "react-select";
import OrderReportTable from "./tables/OrderReportTable";
import ExpenditureReportTable from "./tables/ExpenditureReportTable";
import ReceiptReportTable from "./tables/ReceiptReportTable";
import CapacityReportTable from "./tables/CapacityReportTable";
import StockBalanceReportTable from "./tables/StockBalanceReportTable";
import ClientFactory from "./SDKs/ClientFactory";

type OverviewProps = {
  // using `interface` is also ok
  message: string;
};

type OverviewState = {
  count: number;
  isLoaded: boolean;
  selectedProviderId: string | undefined;
  providers: HealthcareProviderDto[];
  selectedProvider: HealthcareProviderDto | null;
  client: HealthcareProviderClient
};

export default class Overview extends React.Component<
  OverviewProps,
  OverviewState
> {
  options: HealthcareProviderDto[] = [];
  state: OverviewState = {
    isLoaded: false,
    count: 0,
    providers: [],
    selectedProviderId: "",
    selectedProvider: null,
    client: new ClientFactory().CreateProviderClient()
  };

  componentDidMount() {
    const client = this.state.client

    client.getHealthcareProviders(1, 100).then(
      (result) => {
        const initialProvider = result!.data![0];
        this.setState({providers: result.data!, selectedProvider:initialProvider })
      },
      (error) => {
        console.log(error)
        this.setState({
          isLoaded: true,
        });
      }
    );
  }
  private handleChange = (
    selected?: HealthcareProviderDto | HealthcareProviderDto[] | null
  ) => {
    if (selected instanceof HealthcareProviderDto) {
      this.setState({selectedProvider: selected});
    }
  };
  render() {
    return (
    <Container>
        <Row>
            <h2 className="text-center" >Här får du en överblick över vårdgivarnas rapporter</h2>
        </Row>
        <Row className="mt-5">
            <Select
            value={this.state.selectedProvider}
            onChange={this.handleChange}
            options={this.state.providers}
            getOptionLabel={(provider: HealthcareProviderDto) => provider.name!}
            getOptionValue={(provider: HealthcareProviderDto) => provider.id!}
            />
        </Row>
       
        <hr />
        <ReceiptReportTable provider={this.state.selectedProvider!} />
        <hr />
        <StockBalanceReportTable provider={this.state.selectedProvider!} />
        <hr />
        <ExpenditureReportTable provider={this.state.selectedProvider!}  />
        <hr />
        <CapacityReportTable provider={this.state.selectedProvider!} />
        <hr />
        <OrderReportTable
         provider={this.state.selectedProvider!}
         />
        <hr />
   
      </Container>
    );
  }
}
