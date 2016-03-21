﻿using System.Diagnostics;
using NUnit.Framework;

namespace Raygun.Diagnostics.Tests
{
  [TestFixture, RaygunDiagnostics("trace-class", "string-class"), RaygunDiagnosticsUser("johnd-class", "1234-class", "johnd@email.com-class", "John D-class", "John-class")]
  public class TraceListenerFromStringFixture
  {
    private RaygunTraceListener _listener;

    [SetUp]
    public void Init()
    {
      _listener = new RaygunTraceListener();
    }

    [Test]
    public void RaygunExceptionMessageShouldMirrorTraceErrorMessage()
    {
      var context = _listener.MessageFromString("nunit test RaygunExceptionMessageShouldMirrorTraceErrorMessage", "string exception", TraceEventType.Error);
      Assert.That(context, Is.Not.Null);
      Assert.That(context.Exception, Is.Not.Null);
      Assert.That(context.Exception.Message, Does.Contain("nunit test RaygunExceptionMessageShouldMirrorTraceErrorMessage"));
      Assert.That(context.Exception.Message, Does.Contain("string exception"));
    }

    [Test, RaygunDiagnostics("trace-method", "string-method")]
    public void RaygunStringTraceHasTagsAssignedViaMethodAndClassAttributes()
    {
      var context = _listener.MessageFromString("nunit test RaygunHasTagsAssignedViaMethodAndClassAttributes", "string exception", TraceEventType.Error);
      Assert.That(context, Is.Not.Null);
      Assert.That(context.Tags, Is.Not.Null);
      Assert.That(context.Tags.ToArray(), Has.Length.EqualTo(4));
      Assert.That(context.Tags, Has.Exactly(1).Contains("trace-method"));
      Assert.That(context.Tags, Has.Exactly(1).Contains("string-method"));
      Assert.That(context.Tags, Has.Exactly(1).Contains("trace-class"));
      Assert.That(context.Tags, Has.Exactly(1).Contains("string-class"));
    }

    [Test]
    public void RaygunStringTraceHasTagsAssignedViaClassAttribute()
    {
      var context = _listener.MessageFromString("nunit test RaygunHasTagsAssignedViaClassAttribute", "string exception", TraceEventType.Error);
      Assert.That(context, Is.Not.Null);
      Assert.That(context.Tags, Is.Not.Null);
      Assert.That(context.Tags.ToArray(), Has.Length.EqualTo(2));
      Assert.That(context.Tags, Has.Exactly(1).Contains("trace-class"));
      Assert.That(context.Tags, Has.Exactly(1).Contains("string-class"));
    }

    [Test]
    public void MessageFromStringReturnsNullWhenTraceEvenNotErrorOrAbove()
    {
      var context = _listener.MessageFromString("nunit test RaygunHasTagsAssignedViaClassAttribute", "string exception");
      Assert.That(context, Is.Null);
    }

    [Test, RaygunDiagnosticsUser("johnd-method", "1234-method", "johnd@email.com-method", "John D-method", "John-method")]
    public void RaygunStringTraceUserAssignedViaMethodAttribute()
    {
      var context = _listener.MessageFromString("nunit test RaygunHasTagsAssignedViaClassAttribute", "string exception", TraceEventType.Error);
      Assert.That(context.User, Is.Not.Null);
      Assert.That(context.User.Identifier, Is.EqualTo("johnd-method"));
      Assert.That(context.User.Email, Does.Contain("johnd@email.com-method"));
      Assert.That(context.User.UUID, Does.Contain("1234-method"));
      Assert.That(context.User.FullName, Does.Contain("John D-method"));
      Assert.That(context.User.FirstName, Does.Contain("John-method"));
      Assert.That(context.User.IsAnonymous, Is.False);
    }

    [Test]
    public void RaygunStringTraceUserAssignedViaClassAttribute()
    {
      var context = _listener.MessageFromString("nunit test RaygunHasTagsAssignedViaClassAttribute", "string exception", TraceEventType.Error);
      Assert.That(context.User, Is.Not.Null);
      Assert.That(context.User.Identifier, Is.EqualTo("johnd-class"));
      Assert.That(context.User.Email, Does.Contain("johnd@email.com-class"));
      Assert.That(context.User.UUID, Does.Contain("1234-class"));
      Assert.That(context.User.FullName, Does.Contain("John D-class"));
      Assert.That(context.User.FirstName, Does.Contain("John-class"));
      Assert.That(context.User.IsAnonymous, Is.False);
    }
  }
}