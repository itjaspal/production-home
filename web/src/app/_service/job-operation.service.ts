import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthenticationService } from './authentication.service';
import { JobOperationDataTotalView, JobOperationDetailView, JobOperationSearchView } from '../_model/job-operation';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class JobOperationService {

  constructor(
    private http: HttpClient,
    private _authSvc: AuthenticationService,

  ) { 
    
   // this.user = this._authSvc.getLoginUser();

  }

  
  public async searchJobOperationCurrent(_model: JobOperationSearchView) { 

    console.log('searchJobOperationCurrent _model.pageIndex: '+ _model.pageIndex);
    console.log('searchJobOperationCurrent _model.itemPerPage: '+ _model.itemPerPage);
    console.log('searchJobOperationCurrent _model.mc_code: '+ _model.wc_code);
    console.log('searchJobOperationCurrent _model.user_id : '+ _model.user_id);
    console.log('searchJobOperationCurrent _model.build_type : '+ _model.build_type); 
    console.log('searchJobOperationCurrent_model.req_date : '+ _model.req_date); 

    return await this.http.post<JobOperationDetailView<JobOperationDataTotalView>>(environment.API_URL + 'job-operation/postSearchcurrent', _model).toPromise();  

  } 

  public async searchJobOperationPending(_model: JobOperationSearchView) { 

    /*console.log('searchJobOperationCurrent _model.pageIndex: '+ _model.pageIndex);
    console.log('searchJobOperationCurrent _model.itemPerPage: '+ _model.itemPerPage);
    console.log('searchJobOperationCurrent _model.mc_code: '+ _model.wc_code);
    console.log('searchJobOperationCurrent _model.user_id : '+ _model.user_id);
    console.log('searchJobOperationCurrent _model.build_type : '+ _model.build_type); 
    console.log('searchJobOperationCurrent_model.req_date : '+ _model.req_date); */

    return await this.http.post<JobOperationDetailView<JobOperationDataTotalView>>(environment.API_URL + 'job-operation/postSearchpending', _model).toPromise();  

  } 

  public async searchJobOperationForward(_model: JobOperationSearchView) { 

    /*console.log('searchJobOperationCurrent _model.pageIndex: '+ _model.pageIndex);
    console.log('searchJobOperationCurrent _model.itemPerPage: '+ _model.itemPerPage);
    console.log('searchJobOperationCurrent _model.mc_code: '+ _model.wc_code);
    console.log('searchJobOperationCurrent _model.user_id : '+ _model.user_id);
    console.log('searchJobOperationCurrent _model.build_type : '+ _model.build_type); 
    console.log('searchJobOperationCurrent_model.req_date : '+ _model.req_date); */

    return await this.http.post<JobOperationDetailView<JobOperationDataTotalView>>(environment.API_URL + 'job-operation/postSearchforward', _model).toPromise();  

  } 



}  

