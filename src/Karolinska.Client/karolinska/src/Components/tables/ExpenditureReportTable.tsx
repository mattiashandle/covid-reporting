import * as React from "react";
import Table from "react-bootstrap/Table";
import {
  ExpenditureReportDto
} from "../SDKs/api.generated.clients";

type Props = {
  reports: ExpenditureReportDto[];
};

export default class ExpenditureReportTable extends React.Component<Props>
{
  componentDidMount() {
   
  }
  render() {
    return (
      <>
        {/* Förbrukning start*/}
        <div>
        <h3>Förbrukning</h3>
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
                {this.props.reports?.map((report, idx) => {
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
        {/* Förbrukning end*/}
      </>
    )
  }
};

