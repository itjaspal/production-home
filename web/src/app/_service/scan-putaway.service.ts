import { AuthenticationService } from './authentication.service';
import { PrintSetNoView, SetNoSearchView, SetNoView } from './../_model/scan-send';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { productionRecListTotalView, productionRecListDetailView } from '../_model/production-rec-list';
import { PutAwayCancelSearchView, PutAwayDetailSearchView, PutAwayScanFinView, PutAwayScanSearchView, PutAwayScanView, PutAwayTotalView } from '../_model/scan-putaway';

@Injectable({
  providedIn: 'root'
})
export class ScanPutawayService {

  constructor(
    private http: HttpClient,
    private _authSvc: AuthenticationService,
  ) { }

  public async searchPutAwayDetail(_model: PutAwayDetailSearchView) { 
     return await this.http.post<PutAwayTotalView<PutAwayScanView>>(environment.API_URL + 'ScanPutAway/postSearhPtwDetail', _model).toPromise();  
 
   }

  public async searchPutAwayAdd(_model: PutAwayScanSearchView) { 
    return await this.http.post<PutAwayTotalView<PutAwayScanView>>(environment.API_URL + 'ScanPutAway/postScanPutAwayAdd', _model).toPromise();  

  }

  public async searchPutAwayManualAdd(_model: PutAwayScanSearchView) { 
    return await this.http.post<PutAwayTotalView<PutAwayScanView>>(environment.API_URL + 'ScanPutAway/postScanPutAwayManualAdd', _model).toPromise();  

  }

  public async searchPutAwayCancel(_model: PutAwayCancelSearchView) { 
    return await this.http.post<PutAwayTotalView<PutAwayScanView>>(environment.API_URL + 'ScanPutAway/postScanPutAwayCancel', _model).toPromise();  

  }




}
