import Table from "react-bootstrap/Table";
import {
  CapacityReportDto,
  HealthcareProviderDto,
} from "../SDKs/api.generated.clients";
import { useState, useEffect } from "react";
import ClientFactory from "../SDKs/ClientFactory";

type Props = {
  provider: HealthcareProviderDto
}

function CapacityReportTable(props: Props) {
  const [loading, setLoading] = useState(true);
  const [reports, setReports] = useState<CapacityReportDto[]>();
  
  useEffect(() => {
    if(!loading){
      const client = new ClientFactory().CreateProviderClient();
      props.provider && props.provider.id && client.getCapacityReports(props.provider.id, 1, 100).then(response => {
        if(response.data && response.data.length > 1) {
          console.log(response);
          setReports(response.data!)
        }
         
      })
    }
    setLoading(false);
  }, [loading, props]);

   return (
    <>
    {loading ? (<h1>Loading</h1>) : (
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
         {reports?.map((report, idx) => {
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
    )}
    </>
  )
}

export default CapacityReportTable;