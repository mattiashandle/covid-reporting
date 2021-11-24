import Table from "react-bootstrap/Table";
import {
  StockBalanceReportDto,
} from "../sdk/api.generated.clients";

type Props = {
  reports: StockBalanceReportDto[]
}

function StockBalanceReportTableV2(props: Props) {
   return (
    <>
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
         {props.reports?.map((report, idx) => {
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
    </>
  )
}

export default StockBalanceReportTableV2;