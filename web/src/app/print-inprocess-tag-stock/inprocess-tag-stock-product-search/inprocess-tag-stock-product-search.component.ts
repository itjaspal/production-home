import { QcGroupCheckView } from './../../_model/product-defect';
import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { TagProductStockSearchView, TagStockProductSearchView } from '../../_model/print-inprocess-tag-stock';
import { PrintInprocessTagStockService } from '../../_service/print-inprocess-tag-stock.service';

@Component({
  selector: 'app-inprocess-tag-stock-product-search',
  templateUrl: './inprocess-tag-stock-product-search.component.html',
  styleUrls: ['./inprocess-tag-stock-product-search.component.scss']
})

export class InprocessTagStockProductSearchComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<any>,
    @Inject(MAT_DIALOG_DATA) public data: TagStockProductSearchView,
    private _fb: FormBuilder,
    private _tagSvc: PrintInprocessTagStockService,
    private cdr: ChangeDetectorRef,
  ) { }

  public validationForm: FormGroup;
  user:any = {};
  public model : TagStockProductSearchView  = new TagStockProductSearchView();
  //datas: any;
  datas: any = {};
 
  async ngOnInit() {
    //this.buildForm();
    var datePipe = new DatePipe("en-US");
    this.model.entity = this.data.entity;
    this.model.req_date  = datePipe.transform(this.data.req_date, 'dd/MM/yyyy').toString();
    this.model.wc_code = this.data.wc_code;
    this.model.por_no = this.data.por_no;
    this.model.ref_no = this.data.ref_no;
    this.model.group_line = this.data.group_line;
    
    console.log(this.model);
    this.datas = await this._tagSvc.getProduct(this.model);
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
