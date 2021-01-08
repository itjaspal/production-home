import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthenticationService } from './authentication.service';
import { JobOperationDataTotalView, JobOperationDetailView, JobOperationSearchView, ProductionTrackingSearchView, ProductionTrackingView } from '../_model/job-operation';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class JobOperationService {

  constructor(
    private http: HttpClient,
    private _authSvc: AuthenticationService,

  ) { }


  public async searchJobOperationCurrent(_model: JobOperationSearchView) {
    return await this.http.post<JobOperationDetailView<JobOperationDataTotalView>>(environment.API_URL + 'job-operation/postSearchcurrent', _model).toPromise();
  }

  public async searchJobOperationPending(_model: JobOperationSearchView) {
    return await this.http.post<JobOperationDetailView<JobOperationDataTotalView>>(environment.API_URL + 'job-operation/postSearchpending', _model).toPromise();
  }

  public async searchJobOperationForward(_model: JobOperationSearchView) {
    return await this.http.post<JobOperationDetailView<JobOperationDataTotalView>>(environment.API_URL + 'job-operation/postSearchforward', _model).toPromise();
  }

  public async searchProductionTracking(_model: ProductionTrackingSearchView) {
    return await this.http.post<ProductionTrackingView>(environment.API_URL + 'job-operation/productionTracking', _model).toPromise();
  }



}

