﻿using System;
using FluentAssertions;
using NUnit.Framework;

namespace Neo4jClient.Test.GraphClientTests
{
    [TestFixture]
    public class DeleteRelationshipTests
    {
        [Test]
        public void ShouldThrowInvalidOperationExceptionIfNotConnected()
        {
            var client = new GraphClient(new Uri("http://foo"));
            Assert.Throws<InvalidOperationException>(() => client.DeleteRelationship(123));
        }

        [Test]
        public void ShouldDeleteRelationship()
        {
            using (var testHarness = new RestTestHarness
            {
                {
                    MockRequest.Delete("/relationship/456"),
                    MockResponse.Http(204)
                }
            })
            {
                var graphClient = testHarness.CreateAndConnectGraphClient();
                graphClient.DeleteRelationship(456);
            }
        }

        [Test]
        public void ShouldThrowExceptionWhenDeleteFails()
        {
            using (var testHarness = new RestTestHarness
            {
                {
                    MockRequest.Delete("/relationship/456"),
                    MockResponse.Http(404)
                }
            })
            {
                var graphClient = testHarness.CreateAndConnectGraphClient();
                var ex = Assert.Throws<Exception>(() => graphClient.DeleteRelationship(456));
                ex.Message.Should().Be("Unable to delete the relationship. The response status was: 404 NotFound");
            }
        }
    }
}