import * as React from "react";
import {
  Container,
  Row,
  Button,
  Dropdown,
  Form,
  Card,
  Tabs,
  Tab,
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
import { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import ReceiptReportTable from "./tables/ReceiptReportTable";
import StockBalanceReportTable from "./tables/StockBalanceReportTable";
import ExpenditureReportTable from "./tables/ExpenditureReportTable";
import CapacityReportTable from "./tables/CapacityReportTable";
import OrderReportTable from "./tables/OrderReportTable";
import OrderReportForm from "./Forms/OrderReportForm";

type ProviderData = {
  provider: HealthcareProviderDto;
  orderReports: OrderReportDto[] | undefined;
  expenditureReports: ExpenditureReportDto[] | undefined;
  stockBalanceReports: StockBalanceReportDto[] | undefined;
  capacityReports: CapacityReportDto[] | undefined;
  receiptReports: ReceiptReportDto[] | undefined;
};

function ProviderReport() {
  const [providerData, setProviderData] = useState<ProviderData | null>(null);
  const [loading, setLoading] = useState(true);
  const [defaultActiveKey, setDefaultActiveKey] = useState("receipts")

  const query = new URLSearchParams(useLocation().search);

  const id = query.get("id");

  const client = new HealthcareProviderClient("http://localhost:5271");

  const fetchProvider = function (): void {
    if (!loading) return;

    client
      .getHealthcareProvider(id!)
      .then((providerResponse) => {
        Promise.all([
          client.getOrderReports(id!, 1, 100),
          client.getExpenditureReports(id!, 1, 100),
          client.getStockBalanceReports(id!, 1, 100),
          client.getCapacityReports(id!, 1, 100),
          client.getReceiptReports(id!, 1, 100),
        ]).then((reports) => {
          setProviderData({
            provider: providerResponse,
            orderReports: reports[0].data,
            expenditureReports: reports[1].data,
            stockBalanceReports: reports[2].data,
            capacityReports: reports[3].data,
            receiptReports: reports[4].data,
          });
          setLoading(false);
        });
      })
      .catch((error) => {
        console.log(error);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchProvider();
  });

  return (
    <>
      <Container>
        <Row>
          <div>
            {loading ? (
              <h2 className="text-center">Laddar...</h2>
            ) : (
              <div>
                <h2 className="text-center">{providerData && providerData.provider.name}</h2>
                <div>
                  <Tabs
                    defaultActiveKey={defaultActiveKey}
                    id="uncontrolled-tab-example"
                    className="mb-3"
                  >
                    <Tab eventKey="receipts" title="Inleverans">
                      <ReceiptReportTable
                        reports={providerData!.receiptReports!}
                      />
                    </Tab>
                    <Tab eventKey="stockBalance" title="Lagersaldo">
                      <StockBalanceReportTable
                        reports={providerData!.stockBalanceReports!}
                      />
                    </Tab>
                    <Tab eventKey="expenditure" title="Förbrukning">
                      <ExpenditureReportTable
                        reports={providerData!.expenditureReports!}
                      />
                    </Tab>
                    <Tab eventKey="capacity" title="Kapacitet">
                      <CapacityReportTable
                        reports={providerData!.capacityReports!}
                        provider={providerData!.provider!}
                      />
                    </Tab>
                    <Tab eventKey="order" title="Beställning">
                      <Container>
                        {/* <Row className="mt-5">
                          <OrderReportTable
                            // reports={providerData!.orderReports!}
                            provider={providerData!.provider!}
                          />
                        </Row> */}
                        <Row className="mt-5">
                          <OrderReportForm
                            provider={providerData!.provider!}
                          />
                        </Row>
                      </Container>
                    </Tab>
                  </Tabs>
                </div>
              </div>
            )}
          </div>
        </Row>
      </Container>
    </>
  );
}

export default ProviderReport;
