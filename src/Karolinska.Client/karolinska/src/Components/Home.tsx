import {Container, Row } from "react-bootstrap";
import "../App.css";


function Home() {
    return (
      <>
        <Container>
            <Row>
                <div className="Header text-center">
                  <img className="text-center" style={{ width: "100%", maxWidth: "800px" }} alt="region-stockholm" src="https://logovectorseek.com/wp-content/uploads/2020/02/region-stockholm-logo-vector.png" />                
                </div>
            </Row>      
        </Container>
      </>
    );
  }

  export default Home