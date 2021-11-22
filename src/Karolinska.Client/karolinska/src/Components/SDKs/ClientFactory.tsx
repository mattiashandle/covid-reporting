import { HealthcareProviderClient, SupplierClient } from './api.generated.clients';

export default class ClientFactory {
    public CreateProviderClient() {
      return new HealthcareProviderClient("http://localhost:5271"); 
    }

    public CreateSupplierClient(){
      return new SupplierClient("http://localhost:5271");
    }
}