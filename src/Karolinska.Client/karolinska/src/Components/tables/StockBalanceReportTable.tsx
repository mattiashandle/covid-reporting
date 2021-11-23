import Table from "react-bootstrap/Table";
import {
  StockBalanceReportDto,
  HealthcareProviderDto,
} from "../SDKs/api.generated.clients";
import { useState, useEffect } from "react";
import ClientFactory from "../SDKs/ClientFactory";

type Props = {
  provider: HealthcareProviderDto
}

function StockBalanceReportTable(props: Props) {
  const [loading, setLoading] = useState(true);
  const [reports, setReports] = useState<StockBalanceReportDto[] | null>(null);

  useEffect(() => {
    if(!loading){
      const client = new ClientFactory().CreateProviderClient();
      props.provider && props.provider.id && client.getStockBalanceReports(props.provider.id, 1, 100).then(response => {setReports(response.data!)})
    }
    setLoading(false);
  }, [loading, props]);

   return (
    <>
    {loading ? (<h1>Loading</h1>) : (
      <div>
      <h3 className="" >Lagersaldo</h3>
       <Table striped bordered hover responsive>
         <thead>
           <tr key="-1">
            <th>#</th>
            <th>Id</th>
            <th>Registrerad</th>
            <th>Datum tid</th>
            <th>Vaccinleverantör</th>
            <th>Kvantitet (vial)</th>
            <th>Kvantitet (doser)</th>
           </tr>
         </thead>
         <tbody>
         {reports?.map((report, idx) => {
                  return <tr key={idx.toString()}>
                        <td>#</td>
                        <td>{report.id}</td>
                        <td>{report.insertDate?.toDateString()}</td>
                        <td>{report.date?.toDateString()}</td>
                        <td>{report.supplierName}</td>
                        <td>{report.numberOfVials}</td>
                        <td>{report.numberOfDoses}</td>
                        </tr>
              })}
           </tbody>
        </Table>
        </div>
    )}
    </>
  )
}

export default StockBalanceReportTable;