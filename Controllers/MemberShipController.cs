using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using eKirana.Constants;
using eKirana.Data;
using eKirana.Models.SetUp;
using eKirana.ViewModels.SetUp.MemberShipVms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace eKirana.Controllers;
public class MemberShipController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly INotyfService _notification;
    public MemberShipController(ApplicationDbContext context, INotyfService notification)
    {
        _context = context;
        _notification = notification;
    }

    public async Task<IActionResult> Index(MemberShipIndexVm vm)
    {
        var memberShipInfos = await _context.MemberShipHolders.Where(x => (string.IsNullOrEmpty(vm.SearchString) || x.Name.Contains(vm.SearchString) || x.Address.Contains(vm.SearchString) || x.Email.Contains(vm.SearchString)) && (vm.SearchMemberShipStatus == null || x.MemberShipStatus == vm.SearchMemberShipStatus) && (vm.SearchNumber == null || x.Phone.Contains(vm.SearchNumber))).ToListAsync();

        vm.MemberShipInfoVms = memberShipInfos.Select(x => new MemberShipInfoVm()
        {
            MemberShipId = x.Id,
            Name = x.Name,
            Address = x.Address,
            Phone = x.Phone,
            Email = x.Email,
            MemberShipStatus = x.MemberShipStatus,
            RegisteredOn = x.RegisteredOn,
            LastTransaction = x.LastTransaction

        }).ToList();

        return View(vm);
    }

    public async Task<IActionResult> Add()
    {
        var vm = new MemberShipAddVm();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Add(MemberShipAddVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Input.");
            }

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var member = new MemberShipHolder();
            member.Name = vm.Name;
            member.Address = vm.Address;
            member.Phone = vm.Phone;
            member.Email = vm.Email;
            member.MemberShipStatus = MemberShipStatusConstants.Active;
            member.RegisteredOn = DateTime.Now;
            member.LastTransaction = null;

            _context.MemberShipHolders.Add(member);
            await _context.SaveChangesAsync();
            tx.Complete();
            _notification.Success("Membership Registered.");
            return RedirectToAction("Index");

        }
        catch (Exception ex)
        {
            _notification.Error(ex.Message);
            return View(vm);
        }
    }

    public async Task<IActionResult> Edit(long MemberShipId)
    {
        try
        {
            var member = await _context.MemberShipHolders.Where(x => x.Id == MemberShipId).FirstOrDefaultAsync();
            if (member == null)
            {
                throw new Exception("No Member Found.");
            }
            var vm = new MemberShipEditVm();
            vm.Name = member.Name;
            vm.Address = member.Address;
            vm.Phone = member.Phone;
            vm.Email = member.Email;
            vm.MemberShipStatus = member.MemberShipStatus;
            return View(vm);
        }
        catch (Exception ex)
        {
            _notification.Error(ex.Message);
            return RedirectToAction("Index");
        }
    }
    [HttpPost]
    public async Task<IActionResult> Edit(long MemberShipId, MemberShipEditVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid Input.");
            }
            var member = await _context.MemberShipHolders.Where(x => x.Id == MemberShipId).FirstOrDefaultAsync();

            if (member == null)
            {
                throw new Exception("No Member Found.");
            }

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            member.Name = vm.Name;
            member.Address = vm.Address;
            member.Phone = vm.Phone;
            member.Email = vm.Email;
            member.MemberShipStatus = vm.MemberShipStatus;

            await _context.SaveChangesAsync();
            tx.Complete();

            _notification.Success("Member Edited.");
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _notification.Error(ex.Message);
            return View(vm);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(long MemberShipId)
    {
        try
        {
            var member = await _context.MemberShipHolders.Where(x => x.Id == MemberShipId).FirstOrDefaultAsync();
            if (member == null)
            {
                throw new Exception("No Member Found.");
            }

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            _context.MemberShipHolders.Remove(member);
            await _context.SaveChangesAsync();

            tx.Complete();
            _notification.Success("Member Deleted.");
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _notification.Error(ex.Message);
            return RedirectToAction("Index");
        }
    }


    public async Task<IActionResult> GetMemberShipDetails(long MemberShipId)
    {

        var member = await _context.MemberShipHolders.Where(x => x.Id == MemberShipId).FirstOrDefaultAsync();

        if (member != null)
        {
            return Json(new
            {
                memberName = member.Name,
                memberAddress = member.Address,
                memberPhone = member.Phone,
                memberLastTransaction = member.LastTransaction?.ToString("yyyy-MM-dd")
            }
            );
        }

        else
        {
            return Json(new
            {
                error = "No member details found."
            });
        }
    }
}


