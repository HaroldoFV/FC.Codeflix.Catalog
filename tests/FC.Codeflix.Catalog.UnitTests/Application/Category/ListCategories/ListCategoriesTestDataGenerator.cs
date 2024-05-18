using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.ListCategories;

public class ListCategoriesTestDataGenerator
{
    public static IEnumerable<object[]> GetInputsWithoutAllParameter(int time = 14)
    {
        var fixture = new ListCategoriesTestFixture();
        var inputExample = fixture.GetExampleInput();

        for (int i = 0; i < time; i++)
        {
            switch (i % 7)
            {
                case 0:
                    yield return [new ListCategoriesInput()];
                    break;
                case 1:
                    yield return [new ListCategoriesInput(inputExample.Page)];
                    break;
                case 3:
                    yield return
                    [
                        new ListCategoriesInput(
                            inputExample.Page,
                            inputExample.PerPage)
                    ];
                    break;
                case 4:
                    yield return
                    [
                        new ListCategoriesInput(
                            inputExample.Page,
                            inputExample.PerPage,
                            inputExample.Search
                        )
                    ];
                    break;
                case 5:
                    yield return
                    [
                        new ListCategoriesInput(
                            inputExample.Page,
                            inputExample.PerPage,
                            inputExample.Search,
                            inputExample.Sort
                        )
                    ];
                    break;
                case 6:
                    yield return [inputExample];
                    break;
                default:
                    yield return [new ListCategoriesInput()];
                    break;
            }
        }
    }
}