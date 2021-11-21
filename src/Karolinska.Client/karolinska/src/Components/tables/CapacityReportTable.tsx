import * as React from "react";
import Table from "react-bootstrap/Table";
import {
  CapacityReportDto,
  HealthcareProviderClient,
  HealthcareProviderDto
} from "../SDKs/api.generated.clients";

type Props = {
  reports: CapacityReportDto[];
  provider: HealthcareProviderDto
};

export default class CapacityReportTable extends React.Component<Props>
{
  client = new HealthcareProviderClient("http://localhost:5271");

  componentDidMount() {
   
  }
  render() {
    return (
      <>
        {/* Kapictet start*/}
        <div>
        <h3>Kapictet</h3>
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
                {this.props.reports?.map((report, idx) => {
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
        {/* Kapictet end*/}
      </>
    )
  }
};

