import { render } from "@testing-library/react";
import * as React from "react";
import Table from "react-bootstrap/Table";
import {
  OrderReportDto,
  HealthcareProviderClient,
  HealthcareProviderDto,
} from "../SDKs/api.generated.clients";
import { useState, useEffect } from "react";


type Props = {
  provider: HealthcareProviderDto
}

function OrderReportTable(props: Props) {
  const [provider, setProvider] = useState<HealthcareProviderDto>(props.provider);
  const [loading, setLoading] = useState(true);
  const [reports, setReports] = useState<OrderReportDto[] | null>(null);

  useEffect(() => {
    if(!loading){
      const client = new HealthcareProviderClient("http://localhost:5271")
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
         {reports?.map((orderReport, idx) => {
                  return <tr key={idx.toString()}>
                          <td>#</td>
                          <td>{orderReport.id}</td>
                          <td>{orderReport.insertDate?.toDateString()}</td>
                          <td>{orderReport.orderDate?.toDateString()}</td>
                          <td>{orderReport.requestedDeliveryDate?.toDateString()}</td>
                          <td>{orderReport.quantity}</td>
                          <td>{orderReport.glnReceiver}</td>
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