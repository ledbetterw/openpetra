// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       Timotheus Pokorra <tp@tbits.net>
//
// Copyright 2017-2018 by TBits.net
//
// This file is part of OpenPetra.
//
// OpenPetra is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.  If not, see <http://www.gnu.org/licenses/>.
//

$('document').ready(function () {
	display_list();
});

function display_list() {
	api.post('serverMSysMan.asmx/TMaintenanceWebConnector_LoadUsersAndModulePermissions', null).then(function (data) {
		data = JSON.parse(data.data.d);
		// on reload, clear content
		$('#browse_container').html('');
		for (item of data.result.SUser) {
			// format a user for every entry
			format_item(item, data.result);
		}
	})
}

function format_item(item, api_result) {
	var permissions = "";
	api_result.SUserModuleAccessPermission.forEach(function(permissionsRow) {
		if (item['s_user_id_c'] == permissionsRow['s_user_id_c'] && permissionsRow['s_can_access_l'] == true) {
			permissions += permissionsRow['s_module_id_c'] + " ";
		}
	});

	item['permissions'] = permissions;

	let row = format_tpl($('[phantom] .tpl_row').clone(), item);
	let view = format_tpl($('[phantom] .tpl_view').clone(), item);
	row.find('.collapse_col').append(view);
	$('#browse_container').append(row);
}

function open_detail(obj) {
	obj = $(obj);
	if (obj.find('.collapse').is(':visible') ) {return}
	$('.tpl_row .collapse').collapse('hide');
	obj.find('.collapse').collapse('show')
}

function open_edit(s_user_id_c) {
	r = {'AUserId': s_user_id_c};
	api.post('serverMSysMan.asmx/TMaintenanceWebConnector_LoadUserAndModulePermissions', r ).then(function (data) {

		parsed = JSON.parse(data.data.d);
		data = parsed.result;

		let m = format_tpl($('[phantom] .tpl_edit').clone(), data.SUser[0]);

		let user_permissions = [];
		for (let p of data.SUserModuleAccessPermission) {
			if (p.s_can_access_l) {
				user_permissions.push(p.s_module_id_c);
			}
		}

		// generated fields
		m = load_modules(parsed.result.SModule, user_permissions, m);

		$('#modal_space').html(m);
		$('#modal_space .modal').modal('show');
	});
}

function save_entry(obj_modal) {
	let obj = $(obj_modal).closest('.modal');

	// extract information from a jquery object
	let x = extract_data(obj);

	let applied_perm = [];

	obj.find('.permissions').find('.tpl_check').each(function (i, obj) {
		obj = $(obj);
		if (obj.find('input').is(':checked')) {
			applied_perm.push(obj.find('data').attr('value'));
		}
	})

	x['AModulePermissions'] = applied_perm;
	let arguments = translate_to_server(x);

	api.post('serverMSysMan.asmx/TMaintenanceWebConnector_SaveUserAndModulePermissions', arguments).then(function (data) {
		$('.modal').modal('hide');
		display_message(i18next.t('forms.saved'), "success");
	})
}

// used to load all available modules and set checkbox if needed
function load_modules(all_modules, selected_modules, obj) {
	let p = $('<div class="container">');
	for (module of all_modules) {
		let pe = $('[phantom] .tpl_check').clone();
		pe.find('data').attr('value', module['s_module_id_c']);
		pe.find('span').text(module['s_module_name_c']);

		if ($.inArray(module['s_module_id_c'], selected_modules) > -1) {
			pe.find('input').attr('checked', true);
		}
		p.append(pe);
	}
	obj.find('.permissions').html(p);
	return obj;
}
