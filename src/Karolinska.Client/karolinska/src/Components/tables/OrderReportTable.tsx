import { render } from "@testing-library/react";
import * as React from "react";
import Table from "react-bootstrap/Table";
import {
  OrderReportDto,
  HealthcareProviderDto,
} from "../SDKs/api.generated.clients";
import { useState, useEffect } from "react";
import ClientFactory from "../SDKs/ClientFactory";

type Props = {
  provider: HealthcareProviderDto
}

function OrderReportTable(props: Props) {
  const [provider, setProvider] = useState<HealthcareProviderDto>(props.provider);
  const [loading, setLoading] = useState(true);
  const [reports, setReports] = useState<OrderReportDto[] | null>(null);

  useEffect(() => {
    if(!loading){
      const client = new ClientFactory().CreateProviderClient()
      props.provider && props.provider.id && client.getOrderReports(props.provider.id, 1, 100).then(response => setReports(response.data!))
    }
    setLoading(false);
  }, [loading, props]);

   return (
    <>
    {loading ? (<h1>Loading</h1>) : (
      <div>
      <h3 className="text-center" >Beställningar</h3>
       <Table striped bordered hover responsive>
         <thead>
           <tr key="-1">
             <th>#</th>
             <th>Id</th>
             <th>Registrerad</th>
             <th>Beställningsdatum</th>
             <th>Önskat lev datum</th>
             <th>Kvantitet (doser)</th>
             <th>GLN Mottagare</th>
           </tr>
         </thead>
         <tbody>
         {reports?.map((report, idx) => {
                  return <tr key={idx.toString()}>
                          <td>#</td>
                          <td>{report.id}</td>
                          <td>{report.insertDate?.toDateString()}</td>
                          <td>{report.orderDate?.toDateString()}</td>
                          <td>{report.requestedDeliveryDate?.toDateString()}</td>
                          <td>{report.quantity}</td>
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

export default OrderReportTable;