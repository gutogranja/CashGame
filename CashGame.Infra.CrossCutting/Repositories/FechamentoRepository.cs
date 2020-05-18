using OfficeOpenXml;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CashGame.Domain.Interfaces.Repositories;
using CashGame.Domain.Entities.Views;
using OfficeOpenXml.Style;
using System.Drawing;

namespace CashGame.Infra.CrossCutting.Excel.Repositories
{
    public class FechamentoRepository : IFechamentoRepository
    {
        public void ExecutarFechamento(List<ComprarFichaView> lstComprarFichas, List<PagtoJogadorView> lstPagtoJogador, List<CaixinhaView> lstCaixinha, List<RakeView> lstRake)
        {
            double totalLibera = 0;
            double totalCliente = 0;
            double totalCx = 0;
            double totalRake = 0;
            double totalGeral = 0;
            int ct = 3;
            using (ExcelPackage exApp = new ExcelPackage())
            {
                ExcelWorksheet exSheet = exApp.Workbook.Worksheets.Add("Cash Game");
                exSheet.TabColor = Color.Black;
                exSheet.DefaultRowHeight = 12;
                exSheet.Cells["A1:O1"].Merge = true;
                exSheet.Cells["A1:O1"].Style.Border.BorderAround(ExcelBorderStyle.Double, Color.White);
                exSheet.Cells["A1:O1"].Style.Font.Size = 20;
                exSheet.Cells["A1:O1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                exSheet.Cells["A1:O1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                exSheet.Cells["A1:O1"].Style.Fill.BackgroundColor.SetColor(Color.Blue);
                exSheet.Cells["A1:O1"].Style.Font.Color.SetColor(Color.Yellow);
                exSheet.Cells["A1:O1"].Value = "Fechamento do Cash Game";
                exSheet.Cells["B:B"].Style.Numberformat.Format = "#,##0.00";
                exSheet.Row(2).Height = 26;
                foreach (ComprarFichaView cf in lstComprarFichas)
                {
                    exSheet.Cells["A" + ct + ":B" + ct].Merge = true;
                    exSheet.Cells["A" + ct + ":B" + ct].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    exSheet.Cells["A" + ct + ":B" + ct].Style.Border.BorderAround(ExcelBorderStyle.Double, Color.White);
                    exSheet.Cells["A" + ct + ":B" + ct].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    exSheet.Cells["A" + ct + ":B" + ct].Style.Fill.BackgroundColor.SetColor(Color.Black);
                    exSheet.Cells["A" + ct + ":B" + ct].Style.Font.Color.SetColor(Color.Yellow);
                    exSheet.Cells["A" + ct + ":B" + ct].Style.Font.Size = 15;
                    exSheet.Cells["A" + ct + ":B" + ct].Value = cf.Data.ToShortDateString();
                    totalLibera = 0;
                    totalCliente = 0;
                    totalRake = 0;
                    totalCx = 0;
                    totalGeral = 0;
                    exSheet.Row(ct + 1).Height = 8;
                    ct += 2;
                    var pesqCx = lstCaixinha.Where(x => x.Data.Date == cf.Data.Date).ToList();
                    exSheet.Cells["B" + ct + ":B" + (ct + 5 + pesqCx.Count)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    //exSheet.Cells["A" + ct + ":B" + (ct + 4 + pesqCx.Count)].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    exSheet.Cells["A" + ct + ":B" + (ct + 4 + pesqCx.Count)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    exSheet.Cells["A" + ct + ":B" + (ct + 4 + pesqCx.Count)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    exSheet.Cells["A" + ct + ":B" + (ct + 4 + pesqCx.Count)].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    exSheet.Cells["A" + ct + ":B" + (ct + 4 + pesqCx.Count)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    exSheet.Cells["A" + ct + ":B" + (ct + 4 + pesqCx.Count)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    exSheet.Cells["A" + ct + ":B" + (ct + 4 + pesqCx.Count)].Style.Fill.BackgroundColor.SetColor(Color.Aquamarine);
                    exSheet.Cells["A" + ct + ":B" + (ct + 5 + pesqCx.Count)].Style.Font.Bold = true;
                    exSheet.Cells["B" + (ct + 4 + pesqCx.Count)].Style.Font.Color.SetColor(Color.Yellow);
                    exSheet.Cells["B" + (ct + 4 + pesqCx.Count)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    exSheet.Cells["B" + (ct + 4 + pesqCx.Count)].Style.Fill.BackgroundColor.SetColor(Color.Blue);
                    exSheet.Cells["B" + (ct + 4 + pesqCx.Count)].Style.Font.Size = 15;
                    exSheet.Cells["B" + (ct + 4 + pesqCx.Count)].Style.Border.BorderAround(ExcelBorderStyle.Double, Color.Snow);
                    exSheet.Cells["A" + ct].Value = "Liberação de fichas";
                    exSheet.Cells["B" + ct].Value = cf.Valor;
                    totalLibera = cf.Valor;
                    ct++;
                    exSheet.Cells["A" + ct].Value = "Pagamento ao cliente";
                    var pesqJogador = lstPagtoJogador.Where(x => x.Data.Date == cf.Data.Date).ToList();
                    if (pesqJogador.Count > 0)
                        totalCliente = pesqJogador.Sum(x => x.Valor);
                    exSheet.Cells["B" + ct].Value = totalCliente;
                    ct++;
                    exSheet.Cells["A" + ct].Value = "Rake";
                    var pesqRake = lstRake.Where(x => x.DataRetirada.Date == cf.Data.Date).ToList();
                    if (pesqRake.Count > 0)
                        totalRake = pesqRake.Sum(x => x.Valor);
                    exSheet.Cells["B" + ct].Value = totalRake;
                    ct++;
                    if (pesqCx.Count > 0)
                        totalCx = pesqCx.Sum(x => x.Valor);
                    exSheet.Cells["A" + ct].Value = "Caixinhas";
                    exSheet.Cells["B" + ct].Value = totalCx;
                    ct++;
                    foreach (CaixinhaView cx in pesqCx)
                    {
                        exSheet.Cells["A" + ct + ":B" + ct].Style.Font.Color.SetColor(Color.Red);
                        exSheet.Cells["A" + ct].Value = $"Caixinha para {cx.Nome}";
                        exSheet.Cells["B" + ct].Value = cx.Valor;
                        ct++;
                    }
                    totalGeral = totalLibera - totalCliente - totalRake - totalCx;
                    exSheet.Cells["A" + ct].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    exSheet.Cells["A" + ct].Style.Font.Size = 15;
                    exSheet.Cells["A" + ct].Value = "T O T A L";
                    if (totalGeral < 0)
                    {
                        exSheet.Cells["B" + ct].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        exSheet.Cells["B" + ct].Style.Fill.BackgroundColor.SetColor(Color.Red);
                    }
                    exSheet.Cells["B" + ct].Value = totalGeral;
                    ct += 3;
                }
                exSheet.Cells.AutoFitColumns();
                if (File.Exists(@"C:\CashGame.xlsx"))
                    File.Delete(@"C:\CashGame.xlsx");
                exApp.SaveAs(new FileInfo(@"C:\CashGame.xlsx"));
            }
        }
    }
}
