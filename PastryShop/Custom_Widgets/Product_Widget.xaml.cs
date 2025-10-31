namespace PastryShop.Custom_Widgets;

public partial class Product_Widget : ContentPage
{
	public static readonly BindableProperty product_idProperty =
		BindableProperty.Create(nameof(product_id), typeof(int), typeof(Product_Widget), 0);

	public int product_id
	{
		get => (int)GetValue(product_idProperty);
		set => SetValue(product_idProperty, value);
    }

	public static readonly BindableProperty product_nameProperty =
		BindableProperty.Create(nameof(product_name), typeof(string), typeof(Product_Widget), string.Empty);

	public string product_name
	{
		get => (string)GetValue(product_nameProperty);
		set => SetValue(product_nameProperty, value);
	}

	public static readonly BindableProperty product_descriptionProperty = 
		BindableProperty.Create(nameof(product_description), typeof(string), typeof(Product_Widget), string.Empty);

	public string product_description
	{
		get => (string)GetValue(product_descriptionProperty);
		set => SetValue(product_descriptionProperty, value);
	}

	public static readonly BindableProperty product_in_stockProperty =
		BindableProperty.Create(nameof(product_in_stock), typeof(int), typeof(Product_Widget), 0);

	public int product_in_stock
	{
		get => (int)GetValue(product_in_stockProperty);
		set => SetValue(product_in_stockProperty, value);
	}

    public static readonly BindableProperty product_to_buyProperty =
        BindableProperty.Create(nameof(product_to_buy), typeof(int), typeof(Product_Widget), 0);

    public int product_to_buy
    {
        get => (int)GetValue(product_to_buyProperty);
        set => SetValue(product_to_buyProperty, value);
    }

    public static readonly BindableProperty product_priceProperty =
        BindableProperty.Create(nameof(product_price), typeof(int), typeof(Product_Widget), 0, propertyChanged: Price_Changed_);

    public int product_price
    {
        get => (int)GetValue(product_priceProperty);
        set => SetValue(product_priceProperty, value);
    }

    public Product_Widget()
	{
		InitializeComponent();
	}

	public static void Price_Changed_(BindableObject bindable, object old_value, object new_value)
	{
		var control = (Product_Widget)bindable;
		int price = (int)new_value;
		control.price_label.Text = $"{price} pln";
    }

	public void Quantity_Entry_Value_Changed_(object sender, TextChangedEventArgs e)
	{
		if (!(sender is Entry)) { return; }
		Entry entry = (Entry)sender;

		int value = int.TryParse(entry.Text, out int res) ? res : 0;

		if (value < 1) { quantity_to_buy_entry.Text = "1"; return; }
		if (value > product_in_stock) { quantity_to_buy_entry.Text = product_in_stock.ToString(); return; }

        buy_button.Text = $"Add To Cart ({value * product_price} pln)";
    }

	public void Add_To_Cart_(object sender, EventArgs e)
	{

	}
}