import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { JobInProcessStockSearchView, ProductSubSearchView, ScanInprocessSearchView } from '../../_model/job-inprocess-stock';
import { ScanInprocessStockService } from '../../_service/scan-inprocess-stock.service';

@Component({
  selector: 'app-sub-product-search',
  templateUrl: './sub-product-search.component.html',
  styleUrls: ['./sub-product-search.component.scss']
})
export class SubProductSearchComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: ProductSubSearchView,
    private _fb: FormBuilder,
    private _jobInprocessSvc: ScanInprocessStockService,
    private cdr: ChangeDetectorRef,
  ) { }

  public validationForm: FormGroup;
  user:any = {};
  public model : ProductSubSearchView  = new ProductSubSearchView();
  //datas: any;
  datas: any = {};
 

  async ngOnInit() {
    //this.buildForm();
    var datePipe = new DatePipe("en-US");
    this.model.entity = this.data.entity;
    this.model.req_date  = this.data.req_date; 
    this.model.wc_code = this.data.wc_code;
    this.model.por_no = this.data.por_no;
    this.model.ref_no = this.data.ref_no;
    
    console.log(this.model);
    this.datas = await this._jobInprocessSvc.getSubProduct(this.model);
    console.log(this.datas);
    //this.search();
        
  }


  add()
  { 
    let addList: any = this.datas.datas.filter(x => x.selected);
    //console.log(addList);

    this.dialogRef.close(addList);
  }

  close()
  {
    this.dialogRef.close([]);
  }

}
