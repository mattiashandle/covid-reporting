import * as React from "react";
import "../App.css";
import { Container, Row } from "react-bootstrap";
import { CapacityReportDto, ExpenditureReportDto, HealthcareProviderClient, HealthcareProviderDto, OrderReportDto, ReceiptReportDto, StockBalanceReportDto } from "./sdk/api.generated.clients";
import Select from "react-select";
import OrderReportTableV2 from "./tables/OrderReportTableV2";
import ExpenditureReportTableV2 from "./tables/ExpenditureReportTableV2";
import ReceiptReportTableV2 from "./tables/ReceiptReportTableV2";
import CapacityReportTableV2 from "./tables/CapacityReportTableV2";
import StockBalanceReportTableV2 from "./tables/StockBalanceReportTableV2";
import ClientFactory from "./sdk/ClientFactory";

type OverviewProps = {
  message: string;
};

type OverviewState = {
  count: number;
  isLoaded: boolean;
  selectedProviderId: string | undefined;
  providers: HealthcareProviderDto[];
  selectedProvider: HealthcareProviderDto | null;
  client: HealthcareProviderClient,
  capacityReports: CapacityReportDto[],
  orderReports: OrderReportDto[],
  stockBalanceReports: StockBalanceReportDto[],
  expenditureReports: ExpenditureReportDto[],
  receiptReports: ReceiptReportDto[]
};

export default class Overview extends React.Component<
  OverviewProps,
  OverviewState
> {
  
  state: OverviewState = {
    isLoaded: false,
    count: 0,
    providers: [],
    selectedProviderId: "",
    selectedProvider: null,
    client: new ClientFactory().CreateProviderClient(),
    capacityReports: [],
    orderReports: [],
    stockBalanceReports:[],
    expenditureReports: [],
    receiptReports: []
  };

  async fetchData(provider: HealthcareProviderDto) {
    const client = this.state.client
    const capacityReports = await client.getCapacityReports(provider.id!, 1, 100)
    const orderReports = await client.getOrderReports(provider.id!, 1, 100);
    const stockBalanceReports = await client.getStockBalanceReports(provider.id!, 1, 100);
    const expenditureReports = await client.getExpenditureReports(provider.id!, 1, 100);
    const receiptReports = await client.getReceiptReports(provider.id!, 1, 100);
    this.setState({
      selectedProvider: provider, 
      capacityReports: capacityReports.data!,
      orderReports: orderReports.data!,
      stockBalanceReports: stockBalanceReports.data!,
      expenditureReports: expenditureReports.data!,
      receiptReports: receiptReports.data!
     })
  }

  async componentDidMount() {
    const client = this.state.client
    const providersResponse = await client.getHealthcareProviders(1, 100);
    this.setState({providers: providersResponse!.data!})
    const initialProvider = providersResponse!.data![0];
    this.fetchData(initialProvider);
  }

  private handleChange = (
    selected?: HealthcareProviderDto | HealthcareProviderDto[] | null
  ) => {
    if (selected instanceof HealthcareProviderDto) {
      this.fetchData(selected);
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
        <ReceiptReportTableV2 reports={this.state.receiptReports!} />
        <hr />
        <StockBalanceReportTableV2 reports={this.state.stockBalanceReports!} />
        <hr />
        <ExpenditureReportTableV2 reports={this.state.expenditureReports!}  />
        <hr />
        <CapacityReportTableV2 reports={this.state.capacityReports} />
        <hr />
        <OrderReportTableV2 reports={this.state.orderReports}/>
   
      </Container>
    );
  }
}
