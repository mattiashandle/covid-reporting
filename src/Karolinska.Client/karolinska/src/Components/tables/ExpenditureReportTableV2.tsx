import Table from "react-bootstrap/Table";
import { ExpenditureReportDto, HealthcareProviderClient, HealthcareProviderDto } from "../sdk/api.generated.clients";
import { useState, useEffect } from "react";
import ClientFactory from "../sdk/ClientFactory";

type Props = {
  reports: ExpenditureReportDto[] | undefined,
}

function ExpenditureReportTableV2(props: Props) {

   return (
    <>
   
      <div>
      <h3 className="" >Förbrukning</h3>
       <Table striped bordered hover responsive>
         <thead>
           <tr key="-1">
            <th>#</th>
            <th>Id</th>
            <th>Registrerad</th>
            <th>Förbrukningsdatum</th>
            <th>Vaccinleverantör</th>
            <th>Kvantitet (vial)</th>
           </tr>
         </thead>
         <tbody>
         {props.reports?.map((report, idx) => {
                  return <tr key={idx.toString()}>
                          <td>#</td>
                          <td>{report.id}</td>
                          <td>{report.insertDate?.toDateString()}</td>
                          <td>{report.date?.toDateString()}</td>
                          <td>{report.supplierName}</td>
                          <td>{report.numberOfVials}</td>
                        </tr>
              })}
           </tbody>
        </Table>
        </div>
    </>
  )
}

export default ExpenditureReportTableV2;