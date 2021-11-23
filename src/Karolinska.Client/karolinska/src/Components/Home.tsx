import {Container, Row } from "react-bootstrap";
import "../App.css";

function Home() {
    return (
      <>
        <Container>
            <Row>
                <div className="Header text-center">
                    <h2>Syftet är att hålla koll på vaccinsituationen hos Regionens vårdgivare</h2>
                </div>
            </Row>           
        </Container>
      </>
    );
  }

  export default Home