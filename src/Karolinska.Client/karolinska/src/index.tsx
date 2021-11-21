import * as React from "react";
import * as ReactDOM from "react-dom";
import "./index.css";
import App from "./App";
import { Navbar, NavbarBrand, Nav, NavDropdown, Container } from "react-bootstrap";
import { Routes, Route, Link, BrowserRouter } from "react-router-dom";
import Reporting from "./Components/Reporting";
import Overview from "./Components/Overview";

import ProviderReport from "./Components/ProviderReport";
import Home from "./Components/Home";
import 'bootstrap/dist/css/bootstrap.min.css';

ReactDOM.render(
  <Container>
     <Navbar bg="light" expand="lg">
        <Container>
          <Navbar.Brand href="/">Vaccin Portalen</Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <Nav className="me-auto">
              <Nav.Link href="/overview">Översikt</Nav.Link>
              <Nav.Link href="/reports">Inrapportering</Nav.Link>
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
      <App />
      <BrowserRouter>
      <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/reports" element={<Reporting />} />
            <Route path="/overview" element={<Overview message={"hello"} />} />
            <Route path="/reports/add"  element={ <ProviderReport />}/>
          </Routes>
       
      
      </BrowserRouter>
    </Container>,
  document.getElementById("root")
);