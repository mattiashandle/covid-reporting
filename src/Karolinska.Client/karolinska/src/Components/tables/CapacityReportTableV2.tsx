import Table from "react-bootstrap/Table";
import {
  CapacityReportDto,
  HealthcareProviderDto,
} from "../sdk/api.generated.clients";
import { useState, useEffect } from "react";
import ClientFactory from "../sdk/ClientFactory";

// type Props = {
//   provider: HealthcareProviderDto
// }
type CapacityReportTableV2Props = {
  reports: CapacityReportDto[] | null,
}

const CapacityReportTableV2 = (props: CapacityReportTableV2Props) => {
  return (
    <>
      <div>
      <h3 className="" >Kapacitet</h3>
       <Table striped bordered hover responsive>
         <thead>
           <tr key="-1">
           <th>#</th>
            <th>Id</th>
            <th>Registrerad</th>
            <th>Kapacitets datum (prognos)</th>
            <th>Kapacitet (doser)</th>
           </tr>
         </thead>
         <tbody>
         {props.reports?.map((report, idx) => {
                  return <tr key={idx.toString()}>
                          <td>#</td>
                          <td>{report.id}</td>
                          <td>{report.insertDate?.toDateString()}</td>
                          <td>{report.date?.toDateString()}</td>
                          <td>{report.numberOfDoses}</td>
                        </tr>
              })}
           </tbody>
        </Table>
        </div>
    </>
  )
}

export default CapacityReportTableV2;