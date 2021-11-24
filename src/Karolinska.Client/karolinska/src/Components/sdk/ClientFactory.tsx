import { HealthcareProviderClient, SupplierClient } from './api.generated.clients';

export default class ClientFactory {
    public CreateProviderClient() {
      return new HealthcareProviderClient(process.env.REACT_APP_SDK_BASE_URL); 
    }

    public CreateSupplierClient(){
      return new SupplierClient(process.env.REACT_APP_SDK_BASE_URL);
    }
}