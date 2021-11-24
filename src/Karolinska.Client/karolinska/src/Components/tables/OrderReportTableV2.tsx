import Table from "react-bootstrap/Table";
import {
  OrderReportDto,
  HealthcareProviderDto,
} from "../sdk/api.generated.clients";
import { useState, useEffect } from "react";
import ClientFactory from "../sdk/ClientFactory";

type Props = {
  reports?: OrderReportDto[]
}

function OrderReportTableV2(props: Props) {

   return (
    <>
      <div>
      <h3 className="" >Beställningar</h3>
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
         {props.reports?.map((report, idx) => {
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
    </>
  )
}

export default OrderReportTableV2;