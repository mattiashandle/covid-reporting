import Table from "react-bootstrap/Table";
import {
  ReceiptReportDto,
  HealthcareProviderDto,
} from "../SDKs/api.generated.clients";
import { useState, useEffect } from "react";
import ClientFactory from "../SDKs/ClientFactory";

type Props = {
  provider: HealthcareProviderDto
}

function ReceiptReportTable(props: Props) {
  const [provider, setProvider] = useState<HealthcareProviderDto>(props.provider);
  const [loading, setLoading] = useState(true);
  const [reports, setReports] = useState<ReceiptReportDto[] | null>(null);

  useEffect(() => {
    if(!loading){
      const client = new ClientFactory().CreateProviderClient();
      props.provider && props.provider.id && client.getReceiptReports(props.provider.id, 1, 100).then(response => {
        if(response.data && response.data.length > 1) {
          console.log(response);
          setReports(response.data!)
        }
         
      })
    }
    setLoading(false);
  }, [loading, props]);

   return (
    <>
    {loading ? (<h1>Loading</h1>) : (
      <div>
      <h3 className="text-center" >Inleverans</h3>
       <Table striped bordered hover responsive>
         <thead>
           <tr key="-1">
           <th>#</th>
            <th>Id</th>
            <th>Registrerad</th>
            <th>Lev. datum</th>
            <th>Planerat lev datum</th>
            <th>Vaccinleverantör</th>
            <th>Kvantitet (vial)</th>
            <th>GLN Mottagare</th>
           </tr>
         </thead>
         <tbody>
         {reports?.map((report, idx) => {
                  return <tr key={idx.toString()}>
                          <td>#</td>
                          <td>{report.id}</td>
                          <td>{report.insertDate?.toDateString()}</td>
                          <td>{report.deliveryDate?.toDateString()}</td>
                          <td>{report.expectedDeliveryDate?.toDateString()}</td>
                          <td>{report.supplierName}</td>
                          <td>{report.numberOfVials}</td>
                          <td>{report.glnReceiver}</td>
                        </tr>
              })}
           </tbody>
        </Table>
        </div>
    )}
    </>
  )
}

export default ReceiptReportTable;