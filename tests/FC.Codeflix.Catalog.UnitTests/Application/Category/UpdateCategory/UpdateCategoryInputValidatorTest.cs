using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using FluentValidation;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryInputValidatorTest(UpdateCategoryTestFixture fixture)
{
    [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
    [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
    public void DontValidateWhenEmptyGuid()
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        var input = fixture.GetValidInput(Guid.Empty);
        var validator = new UpdateCategoryInputValidator();

        var validateResult = validator.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeFalse();
        validateResult.Errors.Should().HaveCount(1);

        validateResult.Errors[0].ErrorMessage
            .Should().Be("'Id' must not be empty.");
    }


    [Fact(DisplayName = nameof(ValidateWhenValid))]
    [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
    public void ValidateWhenValid()
    {
        var input = fixture.GetValidInput();
        var validator = new UpdateCategoryInputValidator();

        var validateResult = validator.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeTrue();
        validateResult.Errors.Should().HaveCount(0);
    }
}