import {
  Container,
  Row,
  Tabs,
  Tab,
} from "react-bootstrap";
import {
  HealthcareProviderClient,
  HealthcareProviderDto
} from "./SDKs/api.generated.clients";
import { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import ReceiptReportForm from "./Forms/ReceiptReportForm";
import StockBalanceReportForm from "./Forms/StockBalanceReportForm";
import ExpenditureReportForm from "./Forms/ExpenditureReportForm";
import CapacityReportForm from "./Forms/CapacityReportForm";
import OrderReportForm from "./Forms/OrderReportForm";

type ProviderData = {
  provider: HealthcareProviderDto;
};

function ProviderReport() {
  const [providerData, setProviderData] = useState<ProviderData | null>(null);
  const [loading, setLoading] = useState(true);
  const [defaultActiveKey] = useState("receipts")

  const query = new URLSearchParams(useLocation().search);

  const id = query.get("id");

  const client = new HealthcareProviderClient("http://localhost:5271");

  const fetchProvider = function (): void {
    if (!loading) return;
    
    client
      .getHealthcareProvider(id!)
      .then((providerResponse) => {
        setProviderData({provider: providerResponse})
        setLoading(false);
      })
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
                      <Container>
                        <Row className="mt-5">
                          <ReceiptReportForm
                            provider={providerData!.provider!}
                          />
                        </Row>
                      </Container>
                    </Tab>
                    <Tab eventKey="stockBalance" title="Lagersaldo">
                    <Container>
                        <Row className="mt-5">
                          <StockBalanceReportForm
                            provider={providerData!.provider!}
                          />
                        </Row>
                      </Container>
                    </Tab>
                    <Tab eventKey="expenditure" title="Förbrukning">
                    <Container>
                        <Row className="mt-5">
                          <ExpenditureReportForm
                            provider={providerData!.provider!}
                          />
                        </Row>
                      </Container>
                    </Tab>
                    <Tab eventKey="capacity" title="Kapacitet">
                    <Container>
                        <Row className="mt-5">
                          <CapacityReportForm
                            provider={providerData!.provider!}
                          />
                        </Row>
                      </Container>
                    </Tab>
                    <Tab eventKey="order" title="Beställning">
                      <Container>
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
