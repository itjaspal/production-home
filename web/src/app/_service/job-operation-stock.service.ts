import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { JobOperationStockSearchView } from '../_model/job-operation-stock';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class JobOperationStockService {

  constructor(
    private http: HttpClient,
    private _authSvc: AuthenticationService,
  ) { }

  public async searchDataPlan(_model: JobOperationStockSearchView) { 
     return await this.http.post<JobOperationStockSearchView>(environment.API_URL + 'job-operation-stock/postSearchDataPlan', _model).toPromise();   
   } 
}
