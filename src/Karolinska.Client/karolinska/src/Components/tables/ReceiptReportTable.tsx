import * as React from "react";
import Table from "react-bootstrap/Table";
import {
  ReceiptReportDto
} from "../SDKs/api.generated.clients";

type Props = {
  reports: ReceiptReportDto[];
};

export default class ReceiptReportTable extends React.Component<Props>
{
  componentDidMount() {
   
  }
  render() {
    return (
      <>
        {/* Inleverans start*/}
       <div>
        <h3>Inleverans</h3>
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
                {this.props.reports?.map((receiptReport, idx) => {
                    return <tr key={idx.toString()}>
                            <td>#</td>
                            <td>{receiptReport.id}</td>
                            <td>{receiptReport.insertDate?.toDateString()}</td>
                            <td>{receiptReport.deliveryDate?.toDateString()}</td>
                            <td>{receiptReport.expectedDeliveryDate?.toDateString()}</td>
                            <td>{receiptReport.supplierId}</td>
                            <td>{receiptReport.numberOfVials}</td>
                            <td>{receiptReport.glnReceiver}</td>
                           </tr>
                })}
            </tbody>
          </Table>
        </div>
        {/* Inleverans end*/}
      </>
    )
  }
};

