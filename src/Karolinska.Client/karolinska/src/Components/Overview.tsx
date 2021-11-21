import * as React from "react";
import "../App.css";
import { 
    Container, 
    Row, 
    Button, 
    Dropdown 
} from "react-bootstrap";
import {
  CapacityReportDto,
  ExpenditureReportDto,
  HealthcareProviderClient,
  HealthcareProviderDto,
  OrderReportDto,
  ReceiptReportDto,
  StockBalanceReportDto,
} from "./SDKs/api.generated.clients";
import Select from "react-select";
import OrderReportTable from "./tables/OrderReportTable";
import ExpenditureReportTable from "./tables/ExpenditureReportTable";
import ReceiptReportTable from "./tables/ReceiptReportTable";
import CapacityReportTable from "./tables/CapacityReportTable";
import StockBalanceReportTable from "./tables/StockBalanceReportTable";

type OverviewProps = {
  // using `interface` is also ok
  message: string;
};

type OverviewState = {
  count: number;
  isLoaded: boolean;
  selectedProviderId: string | undefined;
  providers: HealthcareProviderDto[];
  orderReports: OrderReportDto[] | undefined;
  expenditureReports: ExpenditureReportDto[] | undefined;
  stockBalanceReports: StockBalanceReportDto[] | undefined;
  capacityReports: CapacityReportDto[] | undefined;
  receiptReports: ReceiptReportDto[] | undefined;
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
    orderReports: [],
    expenditureReports: [],
    stockBalanceReports: [],
    capacityReports: [],
    receiptReports: [],
    selectedProviderId: "",
    selectedProvider: null,
    client: new HealthcareProviderClient("http://localhost:5271")
  };

  loadProviderData(provider: HealthcareProviderDto) {
    const client = this.state.client
    const orderReports = client.getOrderReports(provider.id!, 1, 100);
    const expenditureReports = client.getExpenditureReports(provider.id!, 1, 100);
    const stockBalanceReports = client.getStockBalanceReports(provider.id!, 1, 100);
    const capacityReports = client.getCapacityReports(provider.id!, 1, 100);
    const receiptReports = client.getReceiptReports(provider.id!, 1, 100);

    Promise.all([
      orderReports,
      expenditureReports,
      stockBalanceReports,
      capacityReports,
      receiptReports,
    ]).then((responses) => {
      this.setState({
        selectedProvider: provider,
        orderReports: responses[0].data,
        expenditureReports: responses[1].data,
        stockBalanceReports: responses[2].data,
        capacityReports: responses[3].data,
        receiptReports: responses[4].data,
      });
    });
  }

  componentDidMount() {
    const client = this.state.client

    client.getHealthcareProviders(1, 100).then(
      (result) => {
        const initialProvider = result!.data![0];
        this.setState({providers: result.data!})
        this.loadProviderData(initialProvider)
      },
      // Note: it's important to handle errors here
      // instead of a catch() block so that we don't swallow
      // exceptions from actual bugs in components.
      (error) => {
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
      this.loadProviderData(selected);
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
        <ReceiptReportTable reports={this.state.receiptReports!} />
        <hr />
        <StockBalanceReportTable reports={this.state.stockBalanceReports!} />
        <hr />
        <ExpenditureReportTable reports={this.state.expenditureReports!} />
        <hr />
        <CapacityReportTable provider={this.state.selectedProvider!} reports={this.state.capacityReports!} />
        <hr />
        <OrderReportTable
         provider={this.state.selectedProvider!}
        //  reports={this.state.orderReports!} 
         />
        <hr />
   
      </Container>
    );
  }
}
