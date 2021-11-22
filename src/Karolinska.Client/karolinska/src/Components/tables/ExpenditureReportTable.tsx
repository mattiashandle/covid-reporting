// import * as React from "react";
// import Table from "react-bootstrap/Table";
// import {
//   ExpenditureReportDto
// } from "../SDKs/api.generated.clients";

// type Props = {
//   reports: ExpenditureReportDto[];
// };

// export default class ExpenditureReportTable extends React.Component<Props>
// {
//   componentDidMount() {
   
//   }
//   render() {
//     return (
//       <>
//         {/* Förbrukning start*/}
//         <div>
//         <h3>Förbrukning</h3>
//           <Table striped bordered hover responsive>
//             <thead>
//               <tr key="-1">
//                 <th>#</th>
//                 <th>Id</th>
//                 <th>Registrerad</th>
//                 <th>Förbrukningsdatum</th>
//                 <th>Vaccinleverantör</th>
//                 <th>Kvantitet (vial)</th>
//               </tr>
//             </thead>
//             <tbody>
//                 {this.props.reports?.map((report, idx) => {
//                     return <tr key={idx.toString()}>
//                             <td>#</td>
//                             <td>{report.id}</td>
//                             <td>{report.insertDate?.toDateString()}</td>
//                             <td>{report.date?.toDateString()}</td>
//                             <td>{report.supplierName}</td>
//                             <td>{report.numberOfVials}</td>
//                            </tr>
//                 })}
//             </tbody>
//           </Table>
//         </div>
//         {/* Förbrukning end*/}
//       </>
//     )
//   }
// };

import { render } from "@testing-library/react";
import * as React from "react";
import Table from "react-bootstrap/Table";
import {
  ExpenditureReportDto,
  HealthcareProviderClient,
  HealthcareProviderDto,
} from "../SDKs/api.generated.clients";
import { useState, useEffect } from "react";


type Props = {
  provider: HealthcareProviderDto
}

function ExpenditureReportTable(props: Props) {
  const [provider, setProvider] = useState<HealthcareProviderDto>(props.provider);
  const [loading, setLoading] = useState(true);
  const [reports, setReports] = useState<ExpenditureReportDto[] | null>(null);

  useEffect(() => {
    if(!loading){
      const client = new HealthcareProviderClient("http://localhost:5271")
      props.provider && props.provider.id && client.getExpenditureReports(props.provider.id, 1, 100).then(response => setReports(response.data!))
    }
    setLoading(false);
  }, [loading, props]);

   return (
    <>
    {loading ? (<h1>Loading</h1>) : (
      <div>
      <h3 className="text-center" >Förbrukning</h3>
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
         {reports?.map((report, idx) => {
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
    )}
    </>
  )
}

export default ExpenditureReportTable;