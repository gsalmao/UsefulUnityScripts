public static class Enums
{
    public enum Scenes
    {
        ExampleScene1,
        ExampleScene2,
        ExampleScene3,
        ExampleScene4,
        ExampleScene5,
    }
}

public static class ConstsScenes
{
	public static Dictionary<Enums.Scenes, string> scnEnumToStr = new Dictionary<Enums.Scenes, string>()
	{
		{ Enums.Scenes.ExampleScene1,           ExampleScene1       },
		{ Enums.Scenes.ExampleScene2,           ExampleScene2       },
		{ Enums.Scenes.ExampleScene3,           ExampleScene3		},
		{ Enums.Scenes.ExampleScene4,           ExampleScene4       },
		{ Enums.Scenes.ExampleScene5,           ExampleScene5       }
	};

	public static string ExampleScene1 { get => "ExampleScene1"; }
	public static string ExampleScene2 { get => "ExampleScene2"; }
	public static string ExampleScene3 { get => "ExampleScene3"; }
	public static string ExampleScene4 { get => "ExampleScene4"; }
	public static string ExampleScene5 { get => "ExampleScene5"; }

}