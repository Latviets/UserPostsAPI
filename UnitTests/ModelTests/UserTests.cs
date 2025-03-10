﻿using FluentAssertions;
using FluentValidation;
using UserPostsAPI.Data.Models;

public class UserTests : TestBase
{
    private readonly User _baseModel;

    public UserTests()
    {
        _baseModel = new User
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "123456",
            Address = "123 Street"
        };
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = _baseModel.Clone();
        model.Name = string.Empty;
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Null()
    {
        var model = _baseModel.Clone();
        model.Name = null;
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name"
            && e.ErrorMessage.Contains("Name is required"));
    }


    [Fact]
    public void Should_Have_Error_When_Name_Contains_Numbers()
    {
        var model = _baseModel.Clone();
        model.Name = "John123";
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name" 
            && e.ErrorMessage.Contains("must consist of letters only"));
    }

    [Fact]
    public void Should_Have_Error_When_Name_Contains_Special_Characters()
    {
        var model = _baseModel.Clone();
        model.Name = "John@Doe";
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name" 
            && e.ErrorMessage.Contains("must consist of letters only"));
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Too_Short()
    {
        var model = _baseModel.Clone();
        model.Name = "Jo";
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name" 
            && e.ErrorMessage.Contains("at least 3 characters"));
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_Is_Valid()
    {
        var model = _baseModel.Clone();
        model.Name = "Edvins Valid";
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = _baseModel.Clone();
        model.Email = "invalid-email";
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = _baseModel.Clone();
        model.Email = string.Empty;
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Pass_When_Email_Is_Valid()
    {
        var model = _baseModel.Clone();
        model.Email = "valid@gmail.com";
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = _baseModel.Clone();
        model.Password = string.Empty;
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" 
            && e.ErrorMessage.Contains("Password is required"));
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        var model = _baseModel.Clone();
        model.Password = "123";
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password" 
            && e.ErrorMessage.Contains("at least 6 characters"));
    }

    [Fact]
    public void Should_Pass_When_Password_Is_Valid()
    {
        var model = _baseModel.Clone();
        model.Password = "secure123";
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Have_Error_When_Address_Is_Empty()
    {
        var model = _baseModel.Clone();
        model.Address = string.Empty;
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Address"
            && e.ErrorMessage.Contains("Address is required"));
    }

    [Fact]
    public void Should_Pass_When_Address_Is_Valid()
    {
        var model = _baseModel.Clone();
        model.Address = "123 Elm Street";
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Pass_When_All_Fields_Are_Valid()
    {
        var model = _baseModel.Clone();
        var validator = GetService<IValidator<User>>();

        var result = validator!.Validate(model);

        result.IsValid.Should().BeTrue();
    }
}