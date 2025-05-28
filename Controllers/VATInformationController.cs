using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;

namespace SatraMart.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VATInformationController : ControllerBase
    {
        private readonly string _connString = "Server=DESKTOP-SG845FO\\NAVDEMO;Database=SatraMart;User Id=sa;Password=12345678;TrustServerCertificate=True;";

        [HttpGet("get")]
        public IActionResult GetTop100()
        {
            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    var query = @"SELECT TOP (1000) [COMBINATION], [CUSTREQUEST], [FORMFORMAT], [FORMNUM], [INVOICEDATE],
                                          [INVOICENUM], [PURCHASERNAME], [RETAILTRANSACTIONTABLE], [RETAILTRANSRECIDGROUP],
                                          [SERIALNUM], [TAXCOMPANYADDRESS], [TAXCOMPANYNAME], [TAXREGNUM], [TAXTRANSTXT],
                                          [TRANSTIME], [DATAAREAID], [RECVERSION], [PARTITION], [RECID], [EMAIL],
                                          [PHONE], [CUSTACCOUNT], [CANCEL]
                                 FROM [SatraMart].[dbo].[VASRetailTransVATInformation]";
                    var result = conn.Query<VATInformation>(query).AsList();
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("details")]
        public IActionResult GetByRecId([FromQuery] long recid)
        {
            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    var query = @"
                SELECT [COMBINATION], [CUSTREQUEST], [FORMFORMAT], [FORMNUM],
                       TRY_CAST([INVOICEDATE] AS DATETIME) AS INVOICEDATE,
                       [INVOICENUM], [PURCHASERNAME], [RETAILTRANSACTIONTABLE], 
                       [RETAILTRANSRECIDGROUP], [SERIALNUM], [TAXCOMPANYADDRESS],
                       [TAXCOMPANYNAME], [TAXREGNUM], [TAXTRANSTXT], [TRANSTIME],
                       [DATAAREAID], [RECVERSION], [PARTITION], [RECID], [EMAIL],
                       [PHONE], [CUSTACCOUNT], [CANCEL]
                FROM [SatraMart].[dbo].[VASRetailTransVATInformation]
                WHERE [RECID] = @RecId";

                    var result = conn.QueryFirstOrDefault<VATInformation>(query, new { RecId = recid });

                    if (result == null)
                        return NotFound($"No VAT record found for RECID: {recid}");

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("add")]
        public IActionResult AddVatInfo([FromBody] VATInformation data)
        {
            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    conn.Open();
                    var insertQuery = @"
                        INSERT INTO [SatraMart].[dbo].[VASRetailTransVATInformation] 
                        ([COMBINATION], [CUSTREQUEST], [FORMFORMAT], [FORMNUM], [INVOICEDATE],
                         [INVOICENUM], [PURCHASERNAME], [RETAILTRANSACTIONTABLE], [RETAILTRANSRECIDGROUP],
                         [SERIALNUM], [TAXCOMPANYADDRESS], [TAXCOMPANYNAME], [TAXREGNUM], [TAXTRANSTXT],
                         [TRANSTIME], [DATAAREAID], [RECVERSION], [PARTITION], [EMAIL],
                         [PHONE], [CUSTACCOUNT], [CANCEL])
                        VALUES 
                        (@COMBINATION, @CUSTREQUEST, @FORMFORMAT, @FORMNUM, @INVOICEDATE,
                         @INVOICENUM, @PURCHASERNAME, @RETAILTRANSACTIONTABLE, @RETAILTRANSRECIDGROUP,
                         @SERIALNUM, @TAXCOMPANYADDRESS, @TAXCOMPANYNAME, @TAXREGNUM, @TAXTRANSTXT,
                         @TRANSTIME, @DATAAREAID, @RECVERSION, @PARTITION, @EMAIL,
                         @PHONE, @CUSTACCOUNT, @CANCEL)";

                    var rowsAffected = conn.Execute(insertQuery, data);

                    if (rowsAffected > 0)
                    {
                        return Ok(new { status = "Success", message = "VAT Information added successfully." });
                    }
                    else
                    {
                        return StatusCode(500, new { status = "Error", message = "Failed to insert record." });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }
        [HttpPost("add-with-identity")]
        public IActionResult AddVatInfoWithIdentity([FromBody] VATInformation data)
        {
            try
            {
                using (var conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    // If RECID is identity, exclude it from INSERT and get the generated value
                    var insertQuery = @"
                        INSERT INTO [SatraMart].[dbo].[VASRetailTransVATInformation] 
                        ([COMBINATION], [CUSTREQUEST], [FORMFORMAT], [FORMNUM], [INVOICEDATE],
                         [INVOICENUM], [PURCHASERNAME], [RETAILTRANSACTIONTABLE], [RETAILTRANSRECIDGROUP],
                         [SERIALNUM], [TAXCOMPANYADDRESS], [TAXCOMPANYNAME], [TAXREGNUM], [TAXTRANSTXT],
                         [TRANSTIME], [DATAAREAID], [RECVERSION], [PARTITION], [EMAIL],
                         [PHONE], [CUSTACCOUNT], [CANCEL])
                        VALUES 
                        (@COMBINATION, @CUSTREQUEST, @FORMFORMAT, @FORMNUM, @INVOICEDATE,
                         @INVOICENUM, @PURCHASERNAME, @RETAILTRANSACTIONTABLE, @RETAILTRANSRECIDGROUP,
                         @SERIALNUM, @TAXCOMPANYADDRESS, @TAXCOMPANYNAME, @TAXREGNUM, @TAXTRANSTXT,
                         @TRANSTIME, @DATAAREAID, @RECVERSION, @PARTITION, @EMAIL,
                         @PHONE, @CUSTACCOUNT, @CANCEL);
                        SELECT CAST(SCOPE_IDENTITY() as bigint);";

                    var newRecId = conn.QuerySingle<long>(insertQuery, data);

                    return Ok(new
                    {
                        status = "Success",
                        message = "VAT Information added successfully.",
                        newRecId = newRecId
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "Error", message = ex.Message });
            }
        }
    }
}