import { productionRecListDetailProdView, productionRecListDetailSearchView, productionRecListDetailTotalView, productionRecListDetailView, productionRecListSearchView, productionRecListTotalView } from './../_model/production-rec-list';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthenticationService } from './authentication.service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductionRecListService {

    constructor(
      private http: HttpClient,
      private _authSvc: AuthenticationService,
    ) { }

    public async searchProductionRecList(_model: productionRecListSearchView) { 

      /* console.log('searchJobOperationCurrent _model.pageIndex: '+ _model.pageIndex);
       console.log('searchJobOperationCurrent _model.itemPerPage: '+ _model.itemPerPage);
       console.log('searchJobOperationCurrent _model.mc_code: '+ _model.wc_code);
       console.log('searchJobOperationCurrent _model.user_id : '+ _model.user_id);
       console.log('searchJobOperationCurrent _model.build_type : '+ _model.build_type); 
       console.log('searchJobOperationCurrent_model.req_date : '+ _model.req_date); */
   
       return await this.http.post<productionRecListTotalView<productionRecListDetailView>>(environment.API_URL + 'productionRec/postSearchProductionRec', _model).toPromise();  
   
     }   
     
     public async searchProductionRecListDetail(_model: productionRecListDetailSearchView) { 

      /* console.log('searchJobOperationCurrent _model.pageIndex: '+ _model.pageIndex);
       console.log('searchJobOperationCurrent _model.itemPerPage: '+ _model.itemPerPage);
       console.log('searchJobOperationCurrent _model.mc_code: '+ _model.wc_code);
       console.log('searchJobOperationCurrent _model.user_id : '+ _model.user_id);
       console.log('searchJobOperationCurrent _model.build_type : '+ _model.build_type); 
       console.log('searchJobOperationCurrent_model.req_date : '+ _model.req_date); */
   
       return await this.http.post<productionRecListDetailTotalView<productionRecListDetailProdView>>(environment.API_URL + 'productionRecDetail/postSearchProductionRecDetail', _model).toPromise();  
   
     } 

     public async searchPutAwayWaiting(_model: productionRecListSearchView) { 

       return await this.http.post<productionRecListTotalView<productionRecListDetailView>>(environment.API_URL + 'productionRec/postSearchPutAwayWaiting', _model).toPromise();  
   
     }



}
