import * as React from "react";
import Table from "react-bootstrap/Table";
import {
  StockBalanceReportDto
} from "../SDKs/api.generated.clients";
import ClientFactory from "../SDKs/ClientFactory";

type Props = {
  reports: StockBalanceReportDto[];
};

export default class StockBalanceReportTable extends React.Component<Props>
{
  componentDidMount() {
   
  }
  render() {
    return (
      <>
        {/* Lagersaldo start*/}
       <div>
        <h3>Lagersaldo</h3>
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
                {this.props.reports?.map((report, idx) => {
                    return <tr key={idx.toString()}>
                            <td>#</td>
                            <td>{report.id}</td>
                            <td>{report.insertDate?.toDateString()}</td>
                            <td>{report.date?.toDateString()}</td>
                            <td>{report.supplierName}</td>
                            <td>{report.numberOfVials}</td>
                            <td>{report.numberOfDosages}</td>
                           </tr>
                })}
            </tbody>
          </Table>
        </div>
        {/* Lagersaldo end*/}
      </>
    )
  }
};

