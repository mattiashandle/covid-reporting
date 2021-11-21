import * as React from "react";
import { Routes, Route } from "react-router-dom";
import "./App.css";
import Reporting from "./Components/Reporting";
import Overview from "./Components/Overview";
import Home from "./Components/Home";
import ProviderReport from './Components/ProviderReport';
import {Container, Row } from "react-bootstrap";

function App() {
  return (
    <div>
      <Container>
        <Row>
          <div className="Header text-center">
            <h1>Välkommen till inrapporteringsmodulen av Vaccin till KARDA!</h1>
          </div>
        </Row>
      
      </Container>
    </div>
  );
}
export default App;
