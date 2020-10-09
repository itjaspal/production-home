import { Component, Inject, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ProductSearchView, ProductView } from '../../_model/job-inprocess';
import { ScanInprocessService } from '../../_service/scan-inprocess.service';

@Component({
  selector: 'app-product-cancel-search',
  templateUrl: './product-cancel-search.component.html',
  styleUrls: ['./product-cancel-search.component.scss']
})
export class ProductCancelSearchComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ProductSearchView,
    private _jobInprocessSvc: ScanInprocessService,) { }

    user:any = {};
    public model : ProductSearchView  = new ProductSearchView();
    
    datas: any = {};
    
  
    async ngOnInit() {
      var datePipe = new DatePipe("en-US");
      this.model.req_date  = datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString();
      this.model.pdjit_grp = this.data.pdjit_grp;
      this.model.wc_code = this.data.wc_code;
      
      //console.log(this.model);
      this.datas = await this._jobInprocessSvc.getproductcancel(this.model);
  
      console.log(this.datas);
  
    }
    add()
    { 
      let addList: any = this.datas.datas.filter(x => x.selected);
  
      
      console.log(addList);
  
      this.dialogRef.close(addList);
    }
  
    close()
    {
      this.dialogRef.close([]);
    }

}
