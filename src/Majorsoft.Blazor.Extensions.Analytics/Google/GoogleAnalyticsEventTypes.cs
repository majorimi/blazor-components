namespace Majorsoft.Blazor.Extensions.Analytics.Google
{
	/// <summary>
	/// Event types for Google Analytics
	/// for event and parameter details see: https://developers.google.com/gtagjs/reference/event
	/// </summary>
	public enum GoogleAnalyticsEventTypes
	{
		add_payment_info,
		add_to_cart,
		add_to_wishlist,
		begin_checkout,
		checkout_progress,
		exception,
		generate_lead,
		login,
		page_view,
		purchase,
		refund,
		remove_from_cart,
		screen_view,
		search,
		select_content,
		set_checkout_option,
		share,
		sign_up,
		timing_complete,
		view_item,
		view_item_list,
		view_promotion,
		view_search_results
	}
}